using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleResult
{
    public class BattleResultData
    {
        // コンストラクタ(無処理).
        public BattleResultData()
        {
        }
        // コンストラクタ(コピー).
        public BattleResultData(BattleResultData brd)
        {
            if (brd != null)
            {
                this.ResultDateTime = brd.ResultDateTime;
                this.MapAreaName = brd.MapAreaName;
                this.MapInfoName = brd.MapInfoName;
                this.MapCell = brd.MapCell;
                this.EnemyDeckName = brd.EnemyDeckName;
                this.WinRank = brd.WinRank;
                this.GetShipName = brd.GetShipName;
            }
        }
        
        // 日時.
        public DateTime ResultDateTime { get; set; }
        // 海域.
        public string MapAreaName { get; set; }
        // マップ.
        public string MapInfoName { get; set; }
        // マップセル.
        public int MapCell { get; set; }
        //// 作戦名.
        //public string QuestName { get; set; }
        //// 作戦レベル.
        //public int QuestLevel { get; set; }
        // 敵艦隊名.
        public string EnemyDeckName { get; set; }
        // 戦果ランク.
        public string WinRank { get; set; }
        //// ドロップ艦ID.
        //public int GetShipId { get; set; }
        // ドロップ艦名.
        public string GetShipName { get; set; }

    }
}
