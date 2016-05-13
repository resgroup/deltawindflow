using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RES.CSVReader;
using System.IO;
using Clerc2011;
using System.Globalization;

namespace UncertaintyCalculator
{
    class Program
    {
        private static readonly CultureInfo _culture = new CultureInfo("en-GB");

        static void Main(string[] args)
        {
            CSVReader mastReader = new CSVReader(new FileStream(args[0], FileMode.Open));
            var value = int.Parse(mastReader["MastID"][0]);

            var masts = new Mast[mastReader.RowCount];
            var mastIDs = new int[mastReader.RowCount];

            var weightings = new Dictionary<Turbine, IEnumerable<MastWeighting>>();

            for (int i = 0; i < mastReader.RowCount; i++)
            {
                masts[i] = new Mast(double.Parse(mastReader["MastX"][i], _culture), double.Parse(mastReader["MastY"][i], _culture));
                mastIDs[i] = int.Parse(mastReader["MastID"][i]);
            }

            CSVReader turbineReader = new CSVReader(new FileStream(args[1], FileMode.Open));

            var turbines = new Turbine[turbineReader.RowCount];
            var turbineIDs = new int[turbineReader.RowCount];


            for (int i = 0; i < turbineReader.RowCount; i++)
            {
                turbines[i] = new Turbine(double.Parse(turbineReader["TurbineX"][i], _culture), double.Parse(turbineReader["TurbineY"][i], _culture));
                turbineIDs[i] = int.Parse(turbineReader["TurbineID"][i]);

                var weights = new MastWeighting[mastIDs.Length];

                for (int j = 0; j < mastIDs.Length; j++)
                {
                    var weight = double.Parse(turbineReader["Mast" + mastIDs[j] + "Weight"][i], _culture);

                    weights[j] = new MastWeighting(masts[j], weight);
                }

                weightings.Add(turbines[i], weights);
            }


            var calculator = new TopoUncertaintyCalculator(0.1, 1000, 0.5, weightings, false);

            var binReader = new CSVReader(new FileStream(args[2], FileMode.Open));

            for (int i = 0; i < binReader.RowCount; i++)
            {
                int mastID = int.Parse(binReader["MastID"][i]);
                int turbineID = int.Parse(binReader["TurbineID"][i]);

                int mastIndex = GetIndex(mastIDs, mastID);
                int turbineIndex = GetIndex(turbineIDs, turbineID);

                calculator.AddBin(new BinInformation()
                {
                    BinEnergy = double.Parse(binReader["Energy"][i], _culture),
                    Direction = double.Parse(binReader["Direction"][i], _culture),
                    SpeedUp = double.Parse(binReader["SpeedUp"][i], _culture),
                    SensitivityFactor = double.Parse(binReader["Sensitivity"][i], _culture),
                    Turbine = turbines[turbineIndex],
                    Mast = masts[mastIndex]
                });

            }

            Console.WriteLine(" ");
            Console.WriteLine("Energy:                  {0:0.00}", calculator.Energy);
            Console.WriteLine("Flow Model Uncertainty:  {0:0.00}", calculator.Uncertainty);

        }

        private static int GetIndex(int[] ids, int id)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == id) return i;
            }
            return -1;
        }
    }
}
