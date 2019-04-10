# UniTexturePackerImporter

無料版の TexturePacker でパッキングしたテクスチャを Unity で使用できるようにするエディタ拡張

[![](https://img.shields.io/github/release/baba-s/uni-texture-packer-importer.svg?label=latest%20version)](https://github.com/baba-s/uni-texture-packer-importer/releases)
[![](https://img.shields.io/github/release-date/baba-s/uni-texture-packer-importer.svg)](https://github.com/baba-s/uni-texture-packer-importer/releases)
![](https://img.shields.io/badge/Unity-2018.3%2B-red.svg)
![](https://img.shields.io/badge/.NET-4.x-orange.svg)
[![](https://img.shields.io/github/license/baba-s/uni-texture-packer-importer.svg)](https://github.com/baba-s/uni-texture-packer-importer/blob/master/LICENSE)

## バージョン

- Unity 2018.3.11f1

## 使い方

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190410/20190410200453.png)

無料版の TexturePacker でパッキングしたいテクスチャを設定します  

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190410/20190410200458.png)

「Data Format」を「JSON(Array)」に変更して、  
Unity プロジェクトの Assets フォルダ以下に .json を出力するように「Data file」を設定します  

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190410/20190410200445.png)

Unity プロジェクトの「UniTexturePackerImporterSettings」を選択して、  
TexturePacker でパッキングしたテクスチャを管理するフォルダのパスを「Path List」に追加します  

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190410/20190410200448.png)

そして、TexturePacker でテクスチャをパッキングして Unity プロジェクトに出力すると、  
出力されたテクスチャに分割されたスプライトが設定された状態でインポートされます  