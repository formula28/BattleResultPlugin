using System.Windows.Controls;

namespace BattleResult
{
    /// <summary>
    /// BattleResultView.xaml の相互作用ロジック
    /// </summary>
    public partial class BattleResultView : UserControl
    {
        // コンストラクタ.
        public BattleResultView()
        {
            InitializeComponent();
        }
        //リセットボタンコールバック.
        private void onResetButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ((BattleResultViewModel)this.DataContext).onBattleResultDataReset();
        }
    }
}
