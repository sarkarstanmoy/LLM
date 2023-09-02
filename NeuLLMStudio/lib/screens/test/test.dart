import 'dart:async';
import 'dart:js_interop';
import 'dart:typed_data';

import 'package:animated_text_kit/animated_text_kit.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:neu_llm_studio/common/common.dart';
import 'package:web_socket_channel/web_socket_channel.dart';

import '../../common/prompt_model.dart';
import '../../infrastructure/llama_provider.dart';

class Test extends StatefulWidget {
  const Test({super.key});

  @override
  State<Test> createState() => _TestState();
}

class _TestState extends State<Test> {
  final List<PromptModel> prompts = <PromptModel>[];

  var showLoading = false;
  var showStop = false;
  var _channel = WebSocketChannel.connect(
    Uri.parse('ws://127.0.0.1:8000/chat'),
  );

  @override
  Widget build(BuildContext context) {
    var size = MediaQuery.of(context).size;
    final promptController = TextEditingController();
    return Scaffold(
      appBar: Common().CustomAppBar(),
      body: SafeArea(
        child: Column(
          children: [
            Expanded(
              child: ListView.separated(
                  itemBuilder: (context, index) {
                    return ListTile(
                      title: Text("Prompt: ${prompts[index].question}",
                          maxLines: 100),
                      subtitle:
                          Text("LLM: ${prompts[index].answer}", maxLines: 100),
                    );
                  },
                  separatorBuilder: (context, index) {
                    return Divider();
                  },
                  itemCount: prompts.length),
            ),
            Expanded(
                child: prompts.isEmpty
                    ? Text("Ask Question?")
                    : StreamBuilder(
                        stream: _channel.stream,
                        builder: (context, snapshot) {
                          if (snapshot.hasData) {
                            prompts.last.answer =
                                prompts.last.answer + snapshot.data;
                            prompts.last.answer = prompts.last.answer
                                .replaceAll("''", "")
                                .replaceAll("\\n", "\n");
                          }
                          return Expanded(
                            child: Center(
                                child: SingleChildScrollView(
                                    child: Text(prompts.last.answer,
                                        maxLines: 100))),
                          );
                        },
                      )),
            showLoading ? const CircularProgressIndicator() : Container(),
            showStop
                ? ElevatedButton(
                    onPressed: () {
                      _channel.sink.close();
                    },
                    child: Text("Stop"))
                : Container(),
            Row(
              children: [
                IconButton(
                  onPressed: () {
                    setState(() {
                      prompts.clear();
                      _channel.sink.close();
                    });
                  },
                  icon: const Icon(Icons.refresh),
                ),
                Expanded(
                    child: Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: TextField(
                    controller: promptController,
                    textInputAction: TextInputAction.done,
                    onSubmitted: (value) async {
                      if (_channel.closeCode != null) {
                        _channel = WebSocketChannel.connect(
                          Uri.parse('ws://127.0.0.1:8000/chat'),
                        );
                      }
                      setState(() {
                        showLoading = true;
                      });
                      var promptModel = PromptModel();
                      promptModel.question = value;
                      promptModel.answer = "";
                      _channel.sink.add(value);
                      setState(() {
                        prompts.add(promptModel);
                        showLoading = false;
                        showStop = true;
                      });
                    },
                    decoration: InputDecoration(
                      hintText: 'Send a message',
                      suffixIcon: IconButton(
                          icon: const Icon(Icons.send),
                          onPressed: () {
                            setState(() {
                              //prompts.add(promptController.text);
                            });
                          }),
                      border: OutlineInputBorder(
                          borderSide: BorderSide(
                              width: 3,
                              color: Theme.of(context).colorScheme.onPrimary)),
                    ),
                  ),
                )),
              ],
            ),
          ],
        ),
      ),
    );
  }

  @override
  void dispose() {
    _channel.sink.close();
    super.dispose();
  }
}
