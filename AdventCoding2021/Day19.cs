using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCoding2021
{
    internal class Day19
    {
        public static string A(string input)
        {
            List<Scanner> remainingScanners = new List<Scanner>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<string> scannerLines = new List<string>();
            foreach (string line in lines)
            {
                if (line.StartsWith("---"))
                {
                    if (scannerLines.Count > 0)
                    {
                        remainingScanners.Add(new Scanner(scannerLines));
                        scannerLines = new List<string>();
                    }
                }
                else
                {
                    scannerLines.Add(line);
                }
            }
            remainingScanners.Add(new Scanner(scannerLines));

            // Now we need to find the overlaps
            List<HashSet<IntVector3>> foundScanners = new List<HashSet<IntVector3>>();
            Queue<HashSet<IntVector3>> newFoundScanners = new Queue<HashSet<IntVector3>>();
            foundScanners.Add(remainingScanners[0].orientationPictures[0]);
            newFoundScanners.Enqueue(remainingScanners[0].orientationPictures[0]);

            HashSet<IntVector3> scannerLocations = new HashSet<IntVector3>();
            remainingScanners.RemoveAt(0);
            while (newFoundScanners.Count > 0)
            {
                HashSet<IntVector3> newScanner = newFoundScanners.Dequeue();
                for (int i = remainingScanners.Count - 1; i >= 0; i--)
                {
                    HashSet<IntVector3> newFixedOrientation;
                    IntVector3 scannerLocation = FindMatch(newScanner, remainingScanners[i], out newFixedOrientation);
                    if (scannerLocation != null)
                    {
                        scannerLocations.Add(scannerLocation);
                        foundScanners.Add(newFixedOrientation);
                        newFoundScanners.Enqueue(newFixedOrientation);
                        remainingScanners.RemoveAt(i);
                    }
                }
            }

            // With all overlaps found, we now need to count the points
            HashSet<IntVector3> beacons = new HashSet<IntVector3>();
            foreach (HashSet<IntVector3> foundScanner in foundScanners)
            {
                foreach (IntVector3 foundBeacon in foundScanner)
                {
                    beacons.Add(foundBeacon);
                }
            }

            return beacons.Count().ToString();

        }

        public static string B(string input)
        {
            List<Scanner> remainingScanners = new List<Scanner>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<string> scannerLines = new List<string>();
            foreach (string line in lines)
            {
                if (line.StartsWith("---"))
                {
                    if (scannerLines.Count > 0)
                    {
                        remainingScanners.Add(new Scanner(scannerLines));
                        scannerLines = new List<string>();
                    }
                }
                else
                {
                    scannerLines.Add(line);
                }
            }
            remainingScanners.Add(new Scanner(scannerLines));

            // Now we need to find the overlaps
            List<HashSet<IntVector3>> foundScanners = new List<HashSet<IntVector3>>();
            Queue<HashSet<IntVector3>> newFoundScanners = new Queue<HashSet<IntVector3>>();
            foundScanners.Add(remainingScanners[0].orientationPictures[0]);
            newFoundScanners.Enqueue(remainingScanners[0].orientationPictures[0]);

            HashSet<IntVector3> scannerLocations = new HashSet<IntVector3>();
            remainingScanners.RemoveAt(0);
            while (newFoundScanners.Count > 0)
            {
                HashSet<IntVector3> newScanner = newFoundScanners.Dequeue();
                for(int i = remainingScanners.Count - 1; i >= 0; i--)
                {
                    HashSet<IntVector3> newFixedOrientation;
                    IntVector3 scannerLocation = FindMatch(newScanner, remainingScanners[i], out newFixedOrientation);
                    if (scannerLocation != null)
                    {
                        scannerLocations.Add(scannerLocation);
                        foundScanners.Add(newFixedOrientation);
                        newFoundScanners.Enqueue(newFixedOrientation);
                        remainingScanners.RemoveAt(i);
                    }
                }
            }

            // With all overlaps found, we now need to find the largest distance
            int largestDistance = 0;
            foreach (IntVector3 start in scannerLocations)
            {
                foreach (IntVector3 end in scannerLocations)
                {
                    largestDistance = Math.Max(largestDistance, IntVector3.Distance(start, end));
                }

            }

            return largestDistance.ToString();

        }


        private static IntVector3 FindMatch(HashSet<IntVector3> fixedOrientation, Scanner remainingScanner, out HashSet<IntVector3> newFixedOrientation)
        {
                // look through the available orientations to find a match with one of the fixed orientations
                foreach (HashSet<IntVector3> orientation in remainingScanner.orientationPictures)
                {
                    // align each point with each other point, and check for a match
                    foreach (IntVector3 fixedPoint in fixedOrientation)
                    {
                        foreach (IntVector3 floatingPoint in orientation)
                        {
                            IntVector3 translation = IntVector3.Subtract(fixedPoint, floatingPoint);
                            HashSet<IntVector3> adjustedOrientation = new HashSet<IntVector3>();
                            foreach (IntVector3 floatingBeacon in orientation)
                            {
                                adjustedOrientation.Add(IntVector3.Add(floatingBeacon, translation));
                            }

                            if (MatchCount(fixedOrientation, adjustedOrientation) >= 12)
                            {
                                newFixedOrientation = adjustedOrientation;
                                return translation;
                            }
                        }

                }
            }
            newFixedOrientation = null;
            return null;
        }

        private static int MatchCount(HashSet<IntVector3> a, HashSet<IntVector3> b)
        {
            return a.Intersect(b).Count();
        }
    }

    internal class Scanner
    {
        public List<HashSet<IntVector3>> orientationPictures;

        public Scanner(List<string> input)
        {

            HashSet<IntVector3> originalPicture = new HashSet<IntVector3>();
            foreach (string beaconLocation in input)
            {
                originalPicture.Add(new IntVector3(beaconLocation));
            }

            orientationPictures = new List<HashSet<IntVector3>>();
            orientationPictures.Add(originalPicture);

            HashSet<IntVector3> np;

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.YawLeft().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.YawRight());
            }
            orientationPictures.Add(np);

            // roll left then rotate
            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollLeft().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollLeft().YawLeft().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollLeft().YawRight());
            }
            orientationPictures.Add(np);

            // roll upside down then rotate
            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollLeft().RollLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollLeft().RollLeft().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollLeft().RollLeft().YawLeft().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollLeft().RollLeft().YawRight());
            }
            orientationPictures.Add(np);

            // roll right then rotate
            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollRight());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollRight().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollRight().YawLeft().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.RollRight().YawRight());
            }
            orientationPictures.Add(np);

            // point up then rotate
            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.PitchUp());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.PitchUp().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.PitchUp().YawLeft().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.PitchUp().YawRight());
            }
            orientationPictures.Add(np);

            // point down then rotate
            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.PitchDown());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.PitchDown().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.PitchDown().YawLeft().YawLeft());
            }
            orientationPictures.Add(np);

            np = new HashSet<IntVector3>();
            foreach (IntVector3 i in originalPicture)
            {
                np.Add(i.PitchDown().YawRight());
            }
            orientationPictures.Add(np);
        }

    }
}
