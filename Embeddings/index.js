import { pipeline } from "@xenova/transformers";
let extractor = await pipeline("feature-extraction", "Xenova/all-MiniLM-L6-v2");

let { product } = await calculateSimilarity();
console.log(product);

async function calculateSimilarity() {
  let result = await extractor("This is a simple test.", {
    pooling: "mean",
    normalize: true,
  });
  let result2 = await extractor("I am a good boy", {
    pooling: "mean",
    normalize: true,
  });
  let product = dotProduct(result, result2);
  return { product };
}

function dotProduct(result, result2) {
  let sum = 0;
  for (let index = 0; index < result.data.length; index++) {
    sum += result.data[index] * result2.data[index];
  }
  return sum;
}
