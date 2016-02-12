using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleResult
{
    public interface IBattleResultDataChangeListener
    {
        void onBattleResultDataAdded(BattleResultData brd);
    }
}
