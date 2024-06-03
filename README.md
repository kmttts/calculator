# Calculator
C# .NET8 WPF MVVMアーキテクトを採用した簡易電卓アプリ

## 動作環境
- Windows 10 22H2以降, Windows 11
- .NET 8.0以降
- Visual Studio 2022 17.8以降 (任意)

## 実行方法
- ソースコード取得
    ```bash
    git clone https://github.com/kmttts/calculator.git
    ```

- 使用したライブラリはなし

### Visual Studio
1. Visual StudioでCalculator.slnを開く
2. `Ctrl+F5`または`F5`キーを押下

### dotnetコマンド
```bash
dotnet run --project Calculator [-c Release]
```

## 備考
- `M-`ボタン、`M+`ボタン、`MRC`ボタンの動作は未実装
- テストプロジェクト`CalculatorTest`
  - `MainModel`クラスを簡易的にテストしたもの
