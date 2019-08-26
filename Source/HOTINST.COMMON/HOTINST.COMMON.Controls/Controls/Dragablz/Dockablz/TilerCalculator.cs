﻿using System;
using System.Linq;

namespace HOTINST.COMMON.Controls.Controls.Dragablz.Dockablz
{
    internal static class TilerCalculator
    {
        public static int[] GetCellCountPerColumn(int totalCells)
        {
            if (totalCells == 2)
                return new[] {1, 1};

            var sqrt = System.Math.Sqrt(totalCells);            

            if (unchecked(sqrt == (int) sqrt))
                return Enumerable.Repeat((int) sqrt, (int) sqrt).ToArray();

            var columns = (int)System.Math.Round(sqrt, MidpointRounding.AwayFromZero);
            var minimumCellsPerColumns = (int)System.Math.Floor(sqrt);
            var result = Enumerable.Repeat(minimumCellsPerColumns, columns).ToArray();

            for (var i = columns - 1; result.Aggregate((current, next) => current + next) < totalCells; i--)
                result[i]+=1;

            return result;
        }
    }
}