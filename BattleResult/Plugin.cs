using Grabacr07.KanColleViewer.Composition;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace BattleResult
{
    [Export(typeof(IPlugin))]
    [ExportMetadata("Title", "BattleResult")]
    [ExportMetadata("Description", "戦闘結果表示")]
    [ExportMetadata("Version", "1.0.0")]
    [ExportMetadata("Author", "@formula")]
    [Export(typeof(ITool))]
    [Export(typeof(IRequestNotify))]
    [ExportMetadata("Guid", "43C42ADD-48E0-4506-AD7F-D433CDEBC2AF")]
    class Plugin : IPlugin, ITool, IRequestNotify
    {
        public void Initialize()
        {
#if DEBUG
            Trace.WriteLine("Plugin Initialize", "XXXXX TEST XXXXX");
#endif
            vm = new BattleResultViewModel();
        }
        private BattleResultViewModel vm;
        public string Name => "BattleResult";
        public object View => new BattleResultView() { DataContext = this.vm, };
        public event EventHandler<NotifyEventArgs> NotifyRequested;
    }
}
