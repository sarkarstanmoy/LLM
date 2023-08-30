import 'package:flutter/material.dart';
import 'package:flutter_adaptive_scaffold/flutter_adaptive_scaffold.dart';
import 'package:get/get.dart';
import 'package:get/get_core/src/get_main.dart';
import 'package:neu_llm_studio/screens/offline/offline.dart';
import 'package:neu_llm_studio/screens/test/test.dart';

class Home extends StatefulWidget {
  const Home({super.key});

  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  int _selectedTab = 0;

  @override
  Widget build(BuildContext context) {
    return AdaptiveScaffold(
        useDrawer: false,
        selectedIndex: _selectedTab,
        onSelectedIndexChange: (int index) {
          setState(() {
            _selectedTab = index;
          });
        },
        destinations: [
          NavigationDestination(
            icon: Icon(Icons.bolt_outlined),
            selectedIcon: InkWell(child: Icon(Icons.bolt),onTap: (){ Get.to(Test());},),
            label: 'Test',
          ),
          NavigationDestination(
            icon: Icon(Icons.offline_share_outlined),
            selectedIcon: InkWell(child: Icon(Icons.offline_share,),onTap: (){Get.to(Offline());},),
            label: 'Offline',
          ),
          NavigationDestination(
            icon: Icon(Icons.settings_outlined),
            selectedIcon: Icon(Icons.settings),
            label: 'Settings',
          ),
        ],

      body: (context) {
        if(_selectedTab ==0){
          return Test();
        } else if (_selectedTab ==1){
          return Offline();
        } else {
          return Container();
        }
      },
    );
  }
}
