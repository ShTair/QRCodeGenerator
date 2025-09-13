# QRCodeGenerator

Blazor WebAssembly で実装された QR コード生成アプリです。任意の文字列を入力するとプレビューを表示し、SVG 形式でダウンロードできます。

## 機能
- 誤り訂正レベルの選択（L/M/Q/H）
- 文字列入力フォーム
- 生成された QR コードのプレビュー表示
- SVG ファイルとしての保存

## 必要要件
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

## 使い方
1. リポジトリをクローンします。
2. 次のコマンドでビルドします。
   ```bash
   dotnet build
   ```
3. アプリを起動します。
   ```bash
   dotnet run --project QRCodeGenerator
   ```
4. ブラウザで表示される画面にアクセスし、文字列を入力して QR コードを生成します。

GitHub Pages など任意の静的ホスティングサービスにデプロイすることも可能です。
