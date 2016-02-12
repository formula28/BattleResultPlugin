using Grabacr07.KanColleViewer.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Trace.WriteLine("Plugin Initialize", "XXXXX TEST XXXXX");
        }
        public string Name => "BattleResult";
        public object View => new BattleResultView();
        public event EventHandler<NotifyEventArgs> NotifyRequested;
    }
}
