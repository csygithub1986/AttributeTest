using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest
{
    public enum ESingleOperateType
    {
        分闸, 合闸, 验电, 开门, 关门
    }

    public enum EMultipleOperateType
    {
        断电, 送电
    }

    public enum ETrackElecState
    {
        已断电, 已送电
    }

    public enum EDoorState
    {
        开门, 关门
    }
}
