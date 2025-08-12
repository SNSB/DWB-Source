using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GeoConversion
{
    /// <summary>
    /// This class performs geographic coordinate and datum conversions in both directions.
    /// </summary>
    public class GeoCon
    {

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCon"/> class.
        /// </summary>
        public GeoCon()
        {
        }

        #endregion Construction

        #region Heimeier Transformations

        /* Koordinaten-Transformationen:
         * 
         *       Gauss-Krüger  <-> Geographische (Potsdam)
         *                                 |
         *                         Geographische (WGS84)  <->  UTM  <->  MGRS (UTMREF)
         *                                 |
         *                         Geographische (NAD)
         * 
         * Konvertierungsfunktionen:
         * 
         *          CoordPotsdamGkToGeo  __
         *          CoordPotsdamGeoToGk    |
         *          
         *                         DatumPotsdamToWgs84
         *                         DatumWgs84ToPotsdam
         * 
         *                               __|__      CoordWgs84GeoToUtm    __    CoordWgs84UtmToMgr
         *                                 |        CoordWgs84UtmToGeo          CoordWgs84MgrToUtm
         *   
         *                         DatumWgs84ToNad
         *                         DatumNadToWgs84
         * 
         * 
         */

        /// <summary>
        /// Converts Gauss-Krueger coordinates to Geographic coordinates with Potsdam datum (Bessel ellipsoid).
        /// </summary>
        /// <param name="rechtsWert">Input: Gauss-Krueger Rechtswert.</param>
        /// <param name="hochWert">Input: Gauss-Krueger Hochwert.</param>
        /// <param name="lonPots">Output: Longitude in Potsdam datum.</param>
        /// <param name="latPots">Output: Latitude in Potsdam datum.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool CoordPotsdamGkToGeo(double rechtsWert, double hochWert, ref double lonPots, ref double latPots)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion wandelt GK Koordinaten in geographische Koordinaten
               um. Rechtswert und Hochwert müssen gegeben sein.
               Berechnet werden geographische Länge lonPots und Breite latPots
               im Potsdam Datum.
            */

            // Grenzen des Gauss-Krüger-Systems für Deutschland 46° N < bp < 55° N, 5° E < lp < 16° E
            if (rechtsWert < 2422546 || rechtsWert > 5577454 || hochWert < 5096055 || hochWert > 6228370)  // eigentlich: 6208370, aber zu knapp)
            {
                return false;
            }

            // Rechtswert und Hochwert im Potsdam Datum
            // Große Halbachse a und Abplattung f
            double a = 6377397.155;
            double f = 3.34277321e-3;
            double pi = Math.PI;

            // Polkrümmungshalbmesser c
            double c = a / (1 - f);

            // Quadrat der zweiten numerischen Exzentrizität
            double ex2 = (2 * f - f * f) / ((1 - f) * (1 - f));
            double ex4 = ex2 * ex2;
            double ex6 = ex4 * ex2;
            double ex8 = ex4 * ex4;

            // Koeffizienten zur Berechnung der geographischen Breite aus gegebener
            // Meridianbogenlänge
            double e0 = c * (pi / 180) * (1 - 3 * ex2 / 4 + 45 * ex4 / 64 - 175 * ex6 / 256 + 11025 * ex8 / 16384);
            double f2 = (180 / pi) * (3 * ex2 / 8 - 3 * ex4 / 16 + 213 * ex6 / 2048 - 255 * ex8 / 4096);
            double f4 = (180 / pi) * (21 * ex4 / 256 - 21 * ex6 / 256 + 533 * ex8 / 8192);
            double f6 = (180 / pi) * (151 * ex6 / 6144 - 453 * ex8 / 12288);

            // Geographische Breite bf zur Meridianbogenlänge gf = hw
            double sigma = hochWert / e0;
            double sigmr = sigma * pi / 180;
            double bf = sigma + f2 * Math.Sin(2 * sigmr) + f4 * Math.Sin(4 * sigmr) + f6 * Math.Sin(6 * sigmr);

            // Breite bf in Radianten
            double br = bf * pi / 180;
            double tan1 = Math.Tan(br);
            double tan2 = tan1 * tan1;
            double tan4 = tan2 * tan2;

            double cos1 = Math.Cos(br);
            double cos2 = cos1 * cos1;

            double etasq = ex2 * cos2;

            // Querkrümmungshalbmesser nd
            double nd = c / Math.Sqrt(1 + etasq);
            double nd2 = nd * nd;
            double nd4 = nd2 * nd2;
            double nd6 = nd4 * nd2;
            double nd3 = nd2 * nd;
            double nd5 = nd4 * nd;

            // Längendifferenz dl zum Bezugsmeridian lh
            int kz = Convert.ToInt32(rechtsWert / 1e6 - 0.5);
            double lh = kz * 3;
            double dy = rechtsWert - (kz * 1e6 + 500000);
            double dy2 = dy * dy;
            double dy4 = dy2 * dy2;
            double dy3 = dy2 * dy;
            double dy5 = dy4 * dy;
            double dy6 = dy3 * dy3;

            double b2 = -tan1 * (1 + etasq) / (2 * nd2);
            double b4 = tan1 * (5 + 3 * tan2 + 6 * etasq * (1 - tan2)) / (24 * nd4);
            double b6 = -tan1 * (61 + 90 * tan2 + 45 * tan4) / (720 * nd6);

            double l1 = 1 / (nd * cos1);
            double l3 = -(1 + 2 * tan2 + etasq) / (6 * nd3 * cos1);
            double l5 = (5 + 28 * tan2 + 24 * tan4) / (120 * nd5 * cos1);

            // Geographischer Breite latPots und Länge lonPots als Funktion von Rechts- und Hochwert
            latPots = bf + (180 / pi) * (b2 * dy2 + b4 * dy4 + b6 * dy6);
            lonPots = lh + (180 / pi) * (l1 * dy + l3 * dy3 + l5 * dy5);

            if (lonPots < 5 || lonPots > 16 || latPots < 46 || latPots > 56)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Converts Geographic coordinates with Potsdam datum to Gauss-Krueger coordinates (Bessel ellipsoid).
        /// </summary>
        /// <param name="lonPots">Input: Longitude in Potsdam datum.</param>
        /// <param name="latPots">Input: Latitude in Potsdam datum.</param>
        /// <param name="rechtsWert">Output: Gauss-Krueger Rechtswert.</param>
        /// <param name="hochWert">Output: Gauss-Krueger Hochwert.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool CoordPotsdamGeoToGk(double lonPots, double latPots, ref double rechtsWert, ref double hochWert)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion wandelt geographische Koordinaten in GK Koordinaten
               um. Geographische Länge lonPots und Breite latPots müssen im Potsdam Datum
               gegeben sein. Berechnet werden Rechtswert und Hochwert.
             */

            // Geographische Länge lonPots und Breite latPots im Potsdam Datum
            // Grenzen des Gauss-Krüger-Systems für Deutschland 46° N < bp < 55° N, 5° E < lp < 16° E
            if (latPots < 46 || latPots > 56 || lonPots < 5 || lonPots > 16)
            {
                return false;
            }

            // Potsdam Datum
            // Große Halbachse a und Abplattung f
            double a = 6377397.155;
            double f = 3.34277321e-3;
            double pi = Math.PI;

            // Polkrümmungshalbmesser c
            double c = a / (1 - f);

            // Quadrat der zweiten numerischen Exzentrizität
            double ex2 = (2 * f - f * f) / ((1 - f) * (1 - f));
            double ex4 = ex2 * ex2;
            double ex6 = ex4 * ex2;
            double ex8 = ex4 * ex4;

            // Koeffizienten zur Berechnung der Meridianbogenlänge
            double e0 = c * (pi / 180) * (1 - 3 * ex2 / 4 + 45 * ex4 / 64 - 175 * ex6 / 256 + 11025 * ex8 / 16384);
            double e2 = c * (-3 * ex2 / 8 + 15 * ex4 / 32 - 525 * ex6 / 1024 + 2205 * ex8 / 4096);
            double e4 = c * (15 * ex4 / 256 - 105 * ex6 / 1024 + 2205 * ex8 / 16384);
            double e6 = c * (-35 * ex6 / 3072 + 315 * ex8 / 12288);

            // Breite in Radianten
            double br = latPots * pi / 180;

            double tan1 = Math.Tan(br);
            double tan2 = tan1 * tan1;
            double tan4 = tan2 * tan2;

            double cos1 = Math.Cos(br);
            double cos2 = cos1 * cos1;
            double cos4 = cos2 * cos2;
            double cos3 = cos2 * cos1;
            double cos5 = cos4 * cos1;

            double etasq = ex2 * cos2;

            // Querkrümmungshalbmesser nd
            double nd = c / Math.Sqrt(1 + etasq);

            // Meridianbogenlänge g aus gegebener geographischer Breite bp
            double g = e0 * latPots + e2 * Math.Sin(2 * br) + e4 * Math.Sin(4 * br) + e6 * Math.Sin(6 * br);

            // Längendifferenz dl zum Bezugsmeridian lh
            int kz = Convert.ToInt32((lonPots + 1.5) / 3 - 0.5);
            double lh = kz * 3;
            double dl = (lonPots - lh) * pi / 180;
            double dl2 = dl * dl;
            double dl4 = dl2 * dl2;
            double dl3 = dl2 * dl;
            double dl5 = dl4 * dl;

            // Hochwert und Rechtswert als Funktion von geographischer Breite und Länge
            hochWert = (g + nd * cos2 * tan1 * dl2 / 2 + nd * cos4 * tan1 * (5 - tan2 + 9 * etasq) * dl4 / 24);
            rechtsWert = (nd * cos1 * dl + nd * cos3 * (1 - tan2 + etasq) * dl3 / 6 + nd * cos5 * (5 - 18 * tan2 + tan4) * dl5 / 120 + kz * 1e6 + 500000);

            return true;
        }

        /// <summary>
        /// Converts Geographic coordinates from Potsdam datum to WGS84 datum (Bessel to WGS84 ellipsoid).
        /// </summary>
        /// <param name="lonPots">Input: Longitude in Potsdam datum.</param>
        /// <param name="latPots">Input: Latitude in Potsdam datum.</param>
        /// <param name="lonWgs84">Output: Longitude in WGS84 datum.</param>
        /// <param name="latWgs84">Output: Latitude in WGS84 datum.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool DatumPotsdamToWgs84(double lonPots, double latPots, ref double lonWgs84, ref double latWgs84)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion verschiebt das Kartenbezugssystem (map datum) vom in
               Deutschland gebräuchlichen Potsdam-Datum zum WGS84 (World Geodetic
               System 84) Datum. Geographische Länge lonPots und Breite latPots 
               gemessen in grad auf dem Bessel-Ellipsoid müssen gegeben sein.
               Ausgegeben werden geographische Länge lonWgs84 und
               Breite latWgs84 (in grad) auf dem WGS84-Ellipsoid.
               Bei der Transformation werden die Ellipsoidachsen parallel
               verschoben um dx = 587 m, dy = 16 m und dz = 393 m.
             */

            // Geographische Länge lonPots und Breite latPots im Potsdam Datum
            // Quellsystem Potsdam Datum
            // Große Halbachse a und Abplattung fq
            double a = 6378137.000 - 739.845;
            double fq = 3.35281066e-3 - 1.003748e-05;

            // Zielsystem WGS84 Datum
            // Abplattung f
            double f = 3.35281066e-3;

            // Parameter für datum shift
            double dx = 587;
            double dy = 16;
            double dz = 393;

            // Quadrat der ersten numerischen Exzentrizität in Quell- und Zielsystem
            double e2q = (2 * fq - fq * fq);
            double e2 = (2 * f - f * f);

            // Breite und Länge in Radianten
            double pi = Math.PI;
            double b1 = latPots * (pi / 180);
            double l1 = lonPots * (pi / 180);

            // Querkrümmungshalbmesser nd
            double nd = a / Math.Sqrt(1 - e2q * Math.Sin(b1) * Math.Sin(b1));

            // Kartesische Koordinaten des Quellsystems Potsdam
            double xp = nd * Math.Cos(b1) * Math.Cos(l1);
            double yp = nd * Math.Cos(b1) * Math.Sin(l1);
            double zp = (1 - e2q) * nd * Math.Sin(b1);

            // Kartesische Koordinaten des Zielsystems (datum shift) WGS84
            double x = xp + dx;
            double y = yp + dy;
            double z = zp + dz;

            // Berechnung von Breite und Länge im Zielsystem
            double rb = Math.Sqrt(x * x + y * y);
            double b2 = (180 / pi) * Math.Atan((z / rb) / (1 - e2));

            double l2 = 0;
            if (x > 0)
                l2 = (180 / pi) * Math.Atan(y / x);
            if (x < 0 && y > 0)
                l2 = (180 / pi) * Math.Atan(y / x) + 180;
            if (x < 0 && y < 0)
                l2 = (180 / pi) * Math.Atan(y / x) - 180;

            // Returnwerte zuweisen
            lonWgs84 = l2;
            latWgs84 = b2;

            return true;
        }

        /// <summary>
        /// Converts Geographic coordinates from WGS84 datum to Potsdam datum (WGS84 to Bessel ellipsoid).
        /// </summary>
        /// <param name="lonWgs84">Input: Longitude in WGS84 datum.</param>
        /// <param name="latWgs84">Input: Latitude in WGS84 datum.</param>
        /// <param name="lonPots">Output: Longitude in Potsdam datum.</param>
        /// <param name="latPots">Output: Latitude in Potsdam datum.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool DatumWgs84ToPotsdam(double lonWgs84, double latWgs84, ref double lonPots, ref double latPots)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion verschiebt das Kartenbezugssystem (map datum) vom
               WGS84 Datum (World Geodetic System 84) zum in Deutschland
               gebräuchlichen Potsdam-Datum. Geographische Länge lonWgs84 und Breite
               latWgs84 gemessen in Grad auf dem WGS84 Ellipsoid müssen
               gegeben sein. Ausgegeben werden geographische Länge lonPots
               und Breite latPots (in Grad) auf dem Bessel-Ellipsoid.
               Bei der Transformation werden die Ellipsoidachsen parallel
               verschoben um dx = -587 m, dy = -16 m und dz = -393 m.
             */

            // Geographische Länge lonWgs84 und Breite latWgs84 im WGS84 Datum        
            // Quellsystem WGS84 Datum
            // Große Halbachse a und Abplattung fq
            double a = 6378137.000;
            double fq = 3.35281066e-3;

            // Zielsystem Potsdam Datum
            // Abplattung f
            double f = fq - 1.003748e-5;

            // Parameter für datum shift
            double dx = -587;
            double dy = -16;
            double dz = -393;

            // Quadrat der ersten numerischen Exzentrizität in Quell- und Zielsystem
            double e2q = (2 * fq - fq * fq);
            double e2 = (2 * f - f * f);

            // Breite und Länge in Radianten
            double pi = Math.PI;
            double b1 = latWgs84 * (pi / 180);
            double l1 = lonWgs84 * (pi / 180);

            // Querkrümmungshalbmesser nd
            double nd = a / Math.Sqrt(1 - e2q * Math.Sin(b1) * Math.Sin(b1));

            // Kartesische Koordinaten des Quellsystems WGS84
            double xw = nd * Math.Cos(b1) * Math.Cos(l1);
            double yw = nd * Math.Cos(b1) * Math.Sin(l1);
            double zw = (1 - e2q) * nd * Math.Sin(b1);

            // Kartesische Koordinaten des Zielsystems (datum shift) Potsdam
            double x = xw + dx;
            double y = yw + dy;
            double z = zw + dz;

            // Berechnung von Breite und Länge im Zielsystem
            double rb = Math.Sqrt(x * x + y * y);
            double b2 = (180 / pi) * Math.Atan((z / rb) / (1 - e2));

            double l2 = 0;
            if (x > 0)
                l2 = (180 / pi) * Math.Atan(y / x);
            if (x < 0 && y > 0)
                l2 = (180 / pi) * Math.Atan(y / x) + 180;
            if (x < 0 && y < 0)
                l2 = (180 / pi) * Math.Atan(y / x) - 180;

            lonPots = l2;
            latPots = b2;

            if (lonPots < 5 || lonPots > 16 || latPots < 46 || latPots > 56)
                return false;

            return true;
        }

        /// <summary>
        /// Converts Geographic coordinates from WGS84 datum to NAD27 continental US datum (WGS84 to Clarke ellipsoid).
        /// </summary>
        /// <param name="lonWgs84">Input: Longitude in WGS84 datum.</param>
        /// <param name="latWgs84">Input: Latitude in WGS84 datum.</param>
        /// <param name="lonNad">Output: Longitude in NAD27 datum.</param>
        /// <param name="latNad">Output: Latitude in NAD27 datum.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool DatumWgs84ToNad(double lonWgs84, double latWgs84, ref double lonNad, ref double latNad)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion verschiebt das Kartenbezugssystem (map datum) vom
               WGS84 Datum (World Geodetic System 84) zum in Nordamerika
               gebräuchlichen NAD27(CONUS - continental US)-Datum. Geographische
               Länge lw und Breite bw gemessen in grad auf dem WGS84 Ellipsoid müssen
               gegeben sein. Ausgegeben werden geographische Länge ln
               und Breite bn (in grad) auf dem Clarke-Ellipsoid von 1866.
               Bei der Transformation werden die Ellipsoidachsen parallel
               verschoben um dx = 8 m, dy = -160 m und dz = -176 m.
             */

            // Geographische Länge lonWgs84 und Breite latWgs84 im WGS84 Datum
            // Quellsystem WGS84 Datum
            // Große Halbachse a und Abplattung fq
            double a = 6378137.000;
            double fq = 3.35281066e-3;

            // Zielsystem NAD27(CONUS) Datum
            // Abplattung f
            double f = fq + 3.726464e-5;

            // Parameter für datum shift
            double dx = 8;
            double dy = -160;
            double dz = -176;

            // Quadrat der ersten numerischen Exzentrizität in Quell- und Zielsystem
            double e2q = (2 * fq - fq * fq);
            double e2 = (2 * f - f * f);

            // Breite und Länge in Radianten
            double pi = Math.PI;
            double b1 = latWgs84 * (pi / 180);
            double l1 = lonWgs84 * (pi / 180);

            // Querkrümmungshalbmesser nd
            double nd = a / Math.Sqrt(1 - e2q * Math.Sin(b1) * Math.Sin(b1));

            // Kartesische Koordinaten des Quellsystems WGS84
            double xw = nd * Math.Cos(b1) * Math.Cos(l1);
            double yw = nd * Math.Cos(b1) * Math.Sin(l1);
            double zw = (1 - e2q) * nd * Math.Sin(b1);

            // Kartesische Koordinaten des Zielsystems (datum shift) Potsdam
            double x = xw + dx;
            double y = yw + dy;
            double z = zw + dz;

            // Berechnung von Breite und Länge im Zielsystem
            double rb = Math.Sqrt(x * x + y * y);
            double b2 = (180 / pi) * Math.Atan((z / rb) / (1 - e2));

            double l2 = 0;
            if (x > 0)
                l2 = (180 / pi) * Math.Atan(y / x);
            if (x < 0 && y > 0)
                l2 = (180 / pi) * Math.Atan(y / x) + 180;
            if (x < 0 && y < 0)
                l2 = (180 / pi) * Math.Atan(y / x) - 180;

            lonNad = l2;
            latNad = b2;

            return true;
        }

        /// <summary>
        /// Converts Geographic coordinates from NAD27 continental US datum to WGS84 datum (Clarke to WGS84 ellipsoid).
        /// </summary>
        /// <param name="lonNad">Input: Longitude in NAD27 datum.</param>
        /// <param name="latNad">Input: Latitude in NAD27 datum.</param>
        /// <param name="lonWgs84">Output: Longitude in WGS84 datum.</param>
        /// <param name="latWgs84">Output: Latitude in WGS84 datum.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool DatumNadToWgs84(double lonNad, double latNad, ref double lonWgs84, ref double latWgs84)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion verschiebt das Kartenbezugssystem (map datum) vom in
               Nordamerika gebräuchlichen NAD27(CONUS)-Datum zum WGS84 (World Geodetic
               System 84) Datum. Geographische Länge lonNad und Breite latNad gemessen in
               grad auf dem Clarke Ellipsoid von 1866 müssen gegeben sein.
               Ausgegeben werden geographische Länge lonWgs84 und
               Breite latWgs84 (in grad) auf dem WGS84-Ellipsoid.
               Bei der Transformation werden die Ellipsoidachsen parallel
               verschoben um dx = -8 m, dy = 160 m und dz = 176 m.
             */

            // Geographische Länge ln und Breite bn im NAD27(CONUS) Datum
            // Quellsystem NAD27(CONUS) Datum
            // Große Halbachse a und Abplattung fq
            double a = 6378137.000 + 69.4;
            double fq = 3.35281066e-3 + 3.726464e-05;

            // Zielsystem WGS84 Datum
            //  Abplattung f
            double f = 3.35281066e-3;

            // Parameter für datum shift
            double dx = -8;
            double dy = 160;
            double dz = 176;

            // Quadrat der ersten numerischen Exzentrizität in Quell- und Zielsystem
            double e2q = (2 * fq - fq * fq);
            double e2 = (2 * f - f * f);

            // Breite und Länge in Radianten
            double pi = Math.PI;
            double b1 = latNad * (pi / 180);
            double l1 = lonNad * (pi / 180);

            // Querkrümmungshalbmesser nd
            double nd = a / Math.Sqrt(1 - e2q * Math.Sin(b1) * Math.Sin(b1));

            // Kartesische Koordinaten des Quellsystems NAD27
            double xp = nd * Math.Cos(b1) * Math.Cos(l1);
            double yp = nd * Math.Cos(b1) * Math.Sin(l1);
            double zp = (1 - e2q) * nd * Math.Sin(b1);

            // Kartesische Koordinaten des Zielsystems (datum shift) WGS84
            double x = xp + dx;
            double y = yp + dy;
            double z = zp + dz;

            // Berechnung von Breite und Länge im Zielsystem
            double rb = Math.Sqrt(x * x + y * y);
            double b2 = (180 / pi) * Math.Atan((z / rb) / (1 - e2));

            double l2 = 0;
            if (x > 0)
                l2 = (180 / pi) * Math.Atan(y / x);
            if (x < 0 && y > 0)
                l2 = (180 / pi) * Math.Atan(y / x) + 180;
            if (x < 0 && y < 0)
                l2 = (180 / pi) * Math.Atan(y / x) - 180;

            lonWgs84 = l2;
            latWgs84 = b2;

            return true;
        }

        /// <summary>
        /// Converts Geographic coordinates to UTM coordinates (both WGS84 datum).
        /// </summary>
        /// <param name="lonWgs84">Input: Longitude in WGS84 datum.</param>
        /// <param name="latWgs84">Input: Latitude in WGS84 datum.</param>
        /// <param name="zone">Output: UTM zone.</param>
        /// <param name="ostwert">Output: UTM east value.</param>
        /// <param name="nordwert">Output: UTM north value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool CoordWgs84GeoToUtm(double lonWgs84, double latWgs84, ref string zone, ref double ostwert, ref double nordwert)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion wandelt geographische Koordinaten in UTM Koordinaten
               um. Geographische Länge lonWgs84 und Breite latWgs84 müssen im WGS84 Datum
               gegeben sein. Berechnet werden UTM Zone, Ostwert und Nordwert.
             */

            // Geographische Länge lonWgs84 und Breite latWgs84 im WGS84 Datum
            if (lonWgs84 <= -180 || lonWgs84 > 180 || latWgs84 <= -80 || latWgs84 >= 84)
                // Werte nicht im Bereich des UTM Systems
                return false;

            // WGS84 Datum
            // Große Halbachse a und Abplattung f
            double a = 6378137.000;
            double f = 3.35281068e-3;
            double pi = Math.PI;
            string b_sel = "CDEFGHJKLMNPQRSTUVWXX";

            // Polkrümmungshalbmesser c
            double c = a / (1 - f);

            //Quadrat der zweiten numerischen Exzentrizität
            double ex2 = (2 * f - f * f) / ((1 - f) * (1 - f));
            double ex4 = ex2 * ex2;
            double ex6 = ex4 * ex2;
            double ex8 = ex4 * ex4;

            // Koeffizienten zur Berechnung der Meridianbogenlänge
            double e0 = c * (pi / 180) * (1 - 3 * ex2 / 4 + 45 * ex4 / 64 - 175 * ex6 / 256 + 11025 * ex8 / 16384);
            double e2 = c * (-3 * ex2 / 8 + 15 * ex4 / 32 - 525 * ex6 / 1024 + 2205 * ex8 / 4096);
            double e4 = c * (15 * ex4 / 256 - 105 * ex6 / 1024 + 2205 * ex8 / 16384);
            double e6 = c * (-35 * ex6 / 3072 + 315 * ex8 / 12288);

            // Längenzone lz und Breitenzone (Band) bz
            int lzn = (int)((lonWgs84 + 180) / 6) + 1;
            string lz = lzn.ToString();
            if (lzn < 10)
                lz = "0" + lz;
            int bd = (int)(1 + (latWgs84 + 80) / 8);
            string bz = b_sel.Substring(bd - 1, 1);

            // Geographische Breite in Radianten br
            double br = latWgs84 * pi / 180;

            double tan1 = Math.Tan(br);
            double tan2 = tan1 * tan1;
            double tan4 = tan2 * tan2;

            double cos1 = Math.Cos(br);
            double cos2 = cos1 * cos1;
            double cos4 = cos2 * cos2;
            double cos3 = cos2 * cos1;
            double cos5 = cos4 * cos1;

            double etasq = ex2 * cos2;

            // Querkrümmungshalbmesser nd
            double nd = c / Math.Sqrt(1 + etasq);

            // Meridianbogenlänge g aus gegebener geographischer Breite bw
            double g = (e0 * latWgs84) + (e2 * Math.Sin(2 * br)) + (e4 * Math.Sin(4 * br)) + (e6 * Math.Sin(6 * br));

            // Längendifferenz dl zum Bezugsmeridian lh
            double lh = (lzn - 30) * 6 - 3;
            double dl = (lonWgs84 - lh) * pi / 180;
            double dl2 = dl * dl;
            double dl4 = dl2 * dl2;
            double dl3 = dl2 * dl;
            double dl5 = dl4 * dl;

            // Maßstabsfaktor auf dem Bezugsmeridian bei UTM Koordinaten m = 0.9996
            // Nordwert und Ostwert als Funktion von geographischer Breite und Länge
            if (latWgs84 < 0)
            {
                nordwert = 10e6 + 0.9996 * (g + nd * cos2 * tan1 * dl2 / 2 + nd * cos4 * tan1 * (5 - tan2 + 9 * etasq) * dl4 / 24);
            }
            else
            {
                nordwert = 0.9996 * (g + nd * cos2 * tan1 * dl2 / 2 + nd * cos4 * tan1 * (5 - tan2 + 9 * etasq) * dl4 / 24);
            }
            ostwert = 0.9996 * (nd * cos1 * dl + nd * cos3 * (1 - tan2 + etasq) * dl3 / 6 + nd * cos5 * (5 - 18 * tan2 + tan4) * dl5 / 120) + 500000;
            zone = lz + bz;

            return true;
        }

        /// <summary>
        /// Converts UTM coordinates to Geographic coordinates (both WGS84 datum).
        /// </summary>
        /// <param name="zone">Input: UTM zone.</param>
        /// <param name="ostwert">Input: UTM east value.</param>
        /// <param name="nordwert">Input: UTM north value.</param>
        /// <param name="lonWgs84">Output: Longitude in WGS84 datum.</param>
        /// <param name="latWgs84">Output: Latitude in WGS84 datum.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool CoordWgs84UtmToGeo(string zone, double ostwert, double nordwert, ref double lonWgs84, ref double latWgs84)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion wandelt UTM Koordinaten in geographische Koordinaten
               um. UTM Zone, Ostwert und Nordwert müssen gegeben sein.
               Berechnet werden geographische Länge lonWgs84 und Breite latWgs84 im 
               WGS84 Datum.
             */

            // Zone (XXY), Ostwert und Nordwert im WGS84 Datum
            if (zone.Length < 3)
                return false;
            char zoneY = zone[2];
            int zoneX = Convert.ToInt32(zone.Substring(0, 2));

            // WGS84 Datum
            // Große Halbachse a und Abplattung f
            double a = 6378137.000;
            double f = 3.35281068e-3;
            double pi = Math.PI;

            // Polkrümmungshalbmesser c
            double c = a / (1 - f);

            // Quadrat der zweiten numerischen Exzentrizität
            double ex2 = (2 * f - f * f) / ((1 - f) * (1 - f));
            double ex4 = ex2 * ex2;
            double ex6 = ex4 * ex2;
            double ex8 = ex4 * ex4;

            // Koeffizienten zur Berechnung der geographischen Breite aus gegebener
            // Meridianbogenlänge
            double e0 = c * (pi / 180) * (1 - 3 * ex2 / 4 + 45 * ex4 / 64 - 175 * ex6 / 256 + 11025 * ex8 / 16384);
            double f2 = (180 / pi) * (3 * ex2 / 8 - 3 * ex4 / 16 + 213 * ex6 / 2048 - 255 * ex8 / 4096);
            double f4 = (180 / pi) * (21 * ex4 / 256 - 21 * ex6 / 256 + 533 * ex8 / 8192);
            double f6 = (180 / pi) * (151 * ex6 / 6144 - 453 * ex8 / 12288);

            double m_nw = 0;
            // Entscheidung Nord-/Süd Halbkugel
            if (zoneY >= 'N')
                m_nw = nordwert;
            else
                m_nw = nordwert - 10e6;

            // Geographische Breite bf zur Meridianbogenlänge gf = m_nw
            double sigma = (m_nw / 0.9996) / e0;
            double sigmr = sigma * pi / 180;
            double bf = sigma + f2 * Math.Sin(2 * sigmr) + f4 * Math.Sin(4 * sigmr) + f6 * Math.Sin(6 * sigmr);

            // Breite bf in Radianten
            double br = bf * pi / 180;
            double tan1 = Math.Tan(br);
            double tan2 = tan1 * tan1;
            double tan4 = tan2 * tan2;

            double cos1 = Math.Cos(br);
            double cos2 = cos1 * cos1;

            double etasq = ex2 * cos2;

            // Querkrümmungshalbmesser nd
            double nd = c / Math.Sqrt(1 + etasq);
            double nd2 = nd * nd;
            double nd4 = nd2 * nd2;
            double nd6 = nd4 * nd2;
            double nd3 = nd2 * nd;
            double nd5 = nd4 * nd;

            // Längendifferenz dl zum Bezugsmeridian lh
            double lh = (zoneX - 30) * 6 - 3;
            double dy = (ostwert - 500000) / 0.9996;
            double dy2 = dy * dy;
            double dy4 = dy2 * dy2;
            double dy3 = dy2 * dy;
            double dy5 = dy3 * dy2;
            double dy6 = dy3 * dy3;

            double b2 = -tan1 * (1 + etasq) / (2 * nd2);
            double b4 = tan1 * (5 + 3 * tan2 + 6 * etasq * (1 - tan2)) / (24 * nd4);
            double b6 = -tan1 * (61 + 90 * tan2 + 45 * tan4) / (720 * nd6);

            double l1 = 1 / (nd * cos1);
            double l3 = -(1 + 2 * tan2 + etasq) / (6 * nd3 * cos1);
            double l5 = (5 + 28 * tan2 + 24 * tan4) / (120 * nd5 * cos1);

            // Geographische Breite bw und Länge lw als Funktion von Ostwert ew und Nordwert nw
            latWgs84 = bf + (180 / pi) * (b2 * dy2 + b4 * dy4 + b6 * dy6);
            lonWgs84 = lh + (180 / pi) * (l1 * dy + l3 * dy3 + l5 * dy5);

            return true;
        }

        /// <summary>
        /// Converts UTM coordinates to Military Grid (UTMREF) coordinates (both WGS84 datum).
        /// </summary>
        /// <param name="zone">Input: UTM zone.</param>
        /// <param name="ostwert">Input: UTM east value.</param>
        /// <param name="nordwert">Input: UTM north value.</param>
        /// <param name="raster">Output: Grid.</param>
        /// <param name="ostwertMgr">Output: MGR east value.</param>
        /// <param name="nordwertMgr">Output: MGR north value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool CoordWgs84UtmToMgr(string zone, double ostwert, double nordwert, ref string raster, ref double ostwertMgr, ref double nordwertMgr)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion wandelt zivile UTM Koordinaten in militärische Koordinaten
               um. UTM Zone, Ostwert und Nordwert müssen gegeben sein.
               Zurückgegeben wird das Rasterfeld raster sowie die aus den
               letzten 5 Stellen von Ost- und Nordwert gebildete Koordinatenangabe
               UTMREF.
             */

            // Längenzone zone, Ostwert und Nordwert im WGS84 Datum
            if (zone.Length < 3)
                return false;

            char zoneY = zone[2];
            int zoneX = Convert.ToInt32(zone.Substring(0, 2));

            ostwertMgr = ostwert % 100000;
            nordwertMgr = nordwert % 100000;
            int ew1 = Convert.ToInt32(ostwert - ostwertMgr - 0.5) / 100000;
            int nw1 = Convert.ToInt32(nordwert - nordwertMgr - 0.5) / 100000;

            string m_east = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string m_north = "ABCDEFGHJKLMNPQRSTUV";

            if (zoneX < 1 || zoneX > 60 || zoneY < 'C' || zoneY > 'X')
                return false;

            int i = zoneX % 3;
            int m_ce = 0;
            if (i == 1)
                m_ce = ew1 - 1;
            if (i == 2)
                m_ce = ew1 + 7;
            if (i == 0)
                m_ce = ew1 + 15;

            i = zoneX % 2;
            int m_cn = 0;
            if (i == 1)
                m_cn = 0;
            else
                m_cn = 5;

            i = nw1;
            while (i - 20 >= 0)
                i = i - 20;
            m_cn = m_cn + i;
            if (m_cn > 19)
                m_cn = m_cn - 20;

            raster = zone + m_east[m_ce] + m_north[m_cn];

            return true;
        }

        /// <summary>
        /// Converts Military Grid (UTMREF) coordinates to UTM coordinates (both WGS84 datum).
        /// </summary>
        /// <param name="raster">Input: Grid.</param>
        /// <param name="ostwertMgr">Input: MGR east value.</param>
        /// <param name="nordwertMgr">Input: MGR north value.</param>
        /// <param name="zone">Output: UTM zone.</param>
        /// <param name="ostwert">Output: UTM east value.</param>
        /// <param name="nordwert">Output: UTM north value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool CoordWgs84MgrToUtm(string raster, double ostwertMgr, double nordwertMgr, ref string zone, ref double ostwert, ref double nordwert)
        {
            /* Copyright (c) 2006, HELMUT H. HEIMEIER
               Permission is hereby granted, free of charge, to any person obtaining a
               copy of this software and associated documentation files (the "Software"),
               to deal in the Software without restriction, including without limitation
               the rights to use, copy, modify, merge, publish, distribute, sublicense,
               and/or sell copies of the Software, and to permit persons to whom the
               Software is furnished to do so, subject to the following conditions:
               The above copyright notice and this permission notice shall be included
               in all copies or substantial portions of the Software.
             */
            /* Die Funktion wandelt militärische UTM Koordinaten (MGR oder
               UTMREF) in zivile UTM Koordinaten um.
               UTM Zone zone, raster und utmref müssen gegeben sein.
               In zone muss die aus 2 Ziffern bestehende Längenzone enthaltens ein
               gefolgt von der aus einem Buchstaben bestehenden Bandangabe.
               In raster muss die aus 2 Buchstaben bestehende Kennung für das
               100 km x 100 km Rasterfeld enthalten sein.
               In UTMREF muss der 5 stellige Ostwert stehen gefolgt von einem blank
               und dem 5 stelligen Nordwert.
               Berechnet wird daraus der 7 stellige Ost- und Nordwert im zivilen
               UTM System.
             */

            // Längenzone zone, Ostwert und Nordwert im WGS84 Datum
            if (raster.Length < 5)
                return false;

            string m_east_0 = "STUVWXYZ";
            string m_east_1 = "ABCDEFGH";
            string m_east_2 = "JKLMNPQR";
            string m_north_0 = "FGHJKLMNPQRSTUVABCDE";
            string m_north_1 = "ABCDEFGHJKLMNPQRSTUV";

            zone = raster.Substring(0, 3);
            string r_east = raster.Substring(3, 1);
            string r_north = raster.Substring(4, 1);

            int i = Convert.ToInt32(raster.Substring(0, 2)) % 3;
            int m_ce = 0;
            if (i == 0)
                m_ce = m_east_0.IndexOf(r_east) + 1;
            if (i == 1)
                m_ce = m_east_1.IndexOf(r_east) + 1;
            if (i == 2)
                m_ce = m_east_2.IndexOf(r_east) + 1;
            ostwert = m_ce * 100000 + ostwertMgr;

            i = Convert.ToInt32(raster.Substring(0, 2)) % 2;
            int m_cn = 0;
            if (i == 0)
                m_cn = m_north_0.IndexOf(r_north);
            else
                m_cn = m_north_1.IndexOf(r_north);

            char band = raster[2];
            if (band >= 'N')
            {
                if (band == 'Q' && m_cn < 10)
                    m_cn = m_cn + 20;
                if (band >= 'R')
                    m_cn = m_cn + 20;
                if (band == 'S' && m_cn < 30)
                    m_cn = m_cn + 20;
                if (band >= 'T')
                    m_cn = m_cn + 20;
                if (band == 'U' && m_cn < 50)
                    m_cn = m_cn + 20;
            }
            else
            {
                if (band == 'C' && m_cn < 10)
                    m_cn = m_cn + 20;
                if (band >= 'D')
                    m_cn = m_cn + 20;
                if (band == 'F' && m_cn < 30)
                    m_cn = m_cn + 20;
                if (band >= 'G')
                    m_cn = m_cn + 20;
                if (band == 'H' && m_cn < 50)
                    m_cn = m_cn + 20;
                if (band >= 'J')
                    m_cn = m_cn + 20;
                if (band == 'K' && m_cn < 70)
                    m_cn = m_cn + 20;
                if (band >= 'L')
                    m_cn = m_cn + 20;
            }
            nordwert = m_cn * 100000 + nordwertMgr;

            return true;
        }

        #endregion // Heimeier Transformations

        #region Molodensky Transformation

        /// <summary>
        /// Converts Geographic coordinates from Merchich datum to WGS84 datum (Clarke 1880 to WGS84 ellipsoid).
        /// </summary>
        /// <param name="lonMer">Input: Longitude in Merchich datum.</param>
        /// <param name="latMer">Input: Latitude in Merchich datum.</param>
        /// <param name="lonWgs84">Output: Longitude in WGS84 datum.</param>
        /// <param name="latWgs84">Output: Latitude in WGS84 datum.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool DatumMerchichToWgs84(double lonMer, double latMer, double altMer, ref double lonWgs84, ref double latWgs84)
        {
            // Set conversion parameters for Merchich to WGS84
            double SemiAxis = 6378249.145;
            double Flat = 0.00340754999949228;
            double DeltaA = -112.2;
            double DeltaF = -0.000054739318;
            double DeltaX = 31;
            double DeltaY = 146;
            double DeltaZ = 47;
            // Create GeodeticPosition (ignore height above geoid)
            GeodeticPosition Geo = new GeodeticPosition(lonMer, latMer, altMer);
            // Molodensky transformation
            GeodeticPosition GeoConv = MolodenskyTransform(Geo, SemiAxis, Flat, DeltaA, DeltaF, DeltaX, DeltaY, DeltaZ);

            // Return conversion values
            lonWgs84 = GeoConv.lon;
            latWgs84 = GeoConv.lat;
            return true;
        }

        /*
         * TransformGeodeticPosition
         *
         * Parameters:
         *     from:     The geodetic position to be translated.
         *     from_a:   The semi-major axis of the "from" ellipsoid.
         *     from_f:   Flattening of the "from" ellipsoid.
         *     from_esq: Eccentricity-squared of the "from" ellipsoid.
         *     da:       Change in semi-major axis length (meters); "to" minus "from"    
         *     df:       Change in flattening; "to" minus "from"
         *     dx:       Change in x between "from" and "to" datum.
         *     dy:       Change in y between "from" and "to" datum.
         *     dz:       Change in z between "from" and "to" datum.
         *   
         *      from Merchich  to  WGS84:
         *      from Axis:  from Flat:           from: Esq             delta A 	    delta F 	        dX [m] 	   dY [m] 	  dZ [m] 	  
         *      6378249.2   0.00340754999949228  0.0824832625566273    -112.2       -0.000054739318     31         146        47          
         * 
         * Does not work properly!
         * 

        public GeodeticPosition TransformGeodeticPosition(
            GeodeticPosition from,
            double from_a, double from_f,
            double from_esq, double da, double df,
            double dx, double dy, double dz)
        {
            double slat = Math.Sin (from.lat);
            double clat = Math.Cos (from.lat);
            double slon = Math.Sin (from.lon);
            double clon = Math.Cos (from.lon);
            double ssqlat = slat * slat;
            double adb = 1.0 / (1.0 - from_f);  // "semi-major axis divided by semi-mino axis"

            double dlat, dlon, dh;

            double rn = from_a / Math.Sqrt (1.0 - from_esq * ssqlat);
            double rm = from_a * (1.0 - from_esq) / Math.Pow ((1.0 - from_esq * ssqlat), 1.5);

            dlat = (((((-dx * slat * clon - dy * slat * slon) + dz * clat)
                        + (da * ((rn * from_esq * slat * clat) / from_a)))
                        + (df * (rm * adb + rn / adb) * slat * clat)))
                        / (rm + from.h); 

            dlon = (-dx * slon + dy * clon) / ((rn + from.h) * clat);

            dh = (dx * clat * clon) + (dy * clat * slon) + (dz * slat)              //  double dh = tx * cosLat * cosLng + ty * cosLat * sinLng + tz * sinLat - da +        
                 - (da * (from_a / rn)) + ((df * rn * ssqlat) / adb);               //       sin2Lat * (eFrom_f * da + eFrom_a * df);                                       

            return new GeodeticPosition (from.lon + dlon, from.lat + dlat, from.h + dh);
        }
        */


        /// <summary>
        /// Molodenskies the transform.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="eFrom_a">The e from_a.</param>
        /// <param name="eFrom_f">The e from_f.</param>
        /// <param name="da">The da.</param>
        /// <param name="df">The df.</param>
        /// <param name="tx">The tx.</param>
        /// <param name="ty">The ty.</param>
        /// <param name="tz">The tz.</param>
        /// <returns>GeodeticPosition (longitude, latitude, height above geoid)</returns>
        public GeodeticPosition MolodenskyTransform(GeodeticPosition from, double eFrom_a, double eFrom_f, double da, double df, double tx, double ty, double tz)
        {
            double lat = from.lat * Math.PI / 180;                                              
            double lng = from.lon * Math.PI / 180;                                              
            double sinLat = Math.Sin(lat);                                                      
            double cosLat = Math.Cos(lat);                                                      
            double sinLng = Math.Sin(lng);                                                      
            double cosLng = Math.Cos(lng);                                                      
            double sin2Lat = sinLat * sinLat;                                                   
            double esqr = eFrom_f * (2 - eFrom_f);                                              
            double nu = eFrom_a / Math.Sqrt(1 - esqr * sin2Lat);                                
            double ro = eFrom_a * (1.0 - esqr) / Math.Pow((1.0 - esqr * sin2Lat), 1.5);         
            double dlat = (1 / (ro + from.h)) * (-tx * sinLat * cosLng - ty * sinLat * sinLng   
                + tz * cosLat + da * (nu * esqr * sinLat * cosLat) / eFrom_a                   
                + sinLat * cosLat * df * ((ro / (1 - eFrom_f)) + nu * (1 - eFrom_f)));         
            double dlng = (-tx * sinLng + ty * cosLng) / ((nu + from.h) * cosLat);              
            double dh = tx * cosLat * cosLng + ty * cosLat * sinLng + tz * sinLat - da +        
                sin2Lat * (eFrom_f * da + eFrom_a * df);
            /*
            string str = lat.ToString() + " " +
            lng.ToString() + " " +
            sinLat.ToString() + " " +
            cosLat.ToString() + " " +
            sinLng.ToString() + " " +
            cosLng.ToString() + " " +
            sin2Lat.ToString() + " " +
            esqr.ToString() + " " +
            nu.ToString() + " " +
            ro.ToString() + " " +
            dlat.ToString() + " " +
            dlng.ToString() + " " +
            dh.ToString() + " ";
            if (str == "")
                return null;
             */
            return new GeodeticPosition((lng + dlng) * 180 / Math.PI, (lat + dlat) * 180 / Math.PI, from.h + dh);                
        }                                                                                     
                                                                                                    
        #endregion // Molodensky Transformation

        #region Convert Coordinate To Decimal

        public const double ConvertError = 12345.6;
        private string[] stringSeparators2 = new string[] { "deg", "°", "'", "´", "\"", " " };

        public bool ConvertCoordinateToDecimal(string line, ref double result)
        {
            double degrees = 0;
            double minutes = 0;
            double seconds = 0;
            int direction = 1;
            string line1 = string.Empty;

            // Check if input string is number
            try
            {
                result = Convert.ToDouble(line.Replace(',', '.'), CultureInfo.InvariantCulture);
                return true;
            }
            catch { }

            // Check N 11° 14' 4.20"
            // --- todo ---

            // Scan input for numbers
            string[] resultLines = line.Split(stringSeparators2, StringSplitOptions.RemoveEmptyEntries);

            if (resultLines.Length <= 1)
                return false;

            // Convert to decimal degrees
            // Check 11° 14' 4.20" N
            try
            {
                if (resultLines.Length == 4)
                {
                    degrees = Convert.ToDouble(resultLines[0], CultureInfo.InvariantCulture);
                    minutes = Convert.ToDouble(resultLines[1], CultureInfo.InvariantCulture);
                    seconds = Convert.ToDouble(resultLines[2], CultureInfo.InvariantCulture);
                    if (resultLines[3].StartsWith("S") || resultLines[3].StartsWith("W"))
                        direction = -1;
                }
            }
            catch
            {
                return false;
            }

            // Check Degrees - Minutes - Seconds
            try
            {
                if (resultLines.Length == 3)
                {
                    degrees = Convert.ToDouble(resultLines[0], CultureInfo.InvariantCulture);
                    minutes = Convert.ToDouble(resultLines[1], CultureInfo.InvariantCulture);

                    // Check 11° 14.20' N
                    if (resultLines[2].StartsWith("S") || resultLines[2].StartsWith("W"))
                    {
                        direction = -1;
                    }
                    else if (resultLines[2].StartsWith("N") || resultLines[2].StartsWith("E"))
                    {
                        direction = 1;
                    }
                    else
                    {
                        seconds = Convert.ToDouble(resultLines[2], CultureInfo.InvariantCulture);
                    }
                }
            }
            catch
            {
                return false;
            }

            // Check 11.14° N
            try
            {
                if (resultLines.Length == 2)
                {
                    degrees = Convert.ToDouble(resultLines[0], CultureInfo.InvariantCulture);
                    if (resultLines[1].StartsWith("S") || resultLines[1].StartsWith("W"))
                        direction = -1;
                }
            }
            catch
            {
                return false;
            }

            // Calculate decimal degrees
            result = (degrees + (minutes / 60.0) + (seconds / 3600)) * direction;
            return true;
        }

        // Markus 8.7.22 - conversion with predefined entities
        public static bool ConvertCoordinateToDecimal(int Deg, int Min, double Sec, ref double Numeric)
        {
            try
            {
                if (Deg * Math.Sign(Deg) > 180 || Min >= 60 || Min < 0 || Sec < 0 || Sec >= 60)
                {
                    return false;
                }
                double dDeg = (double)Deg * Math.Sign(Deg);
                double dMin = (double)Min / (double)60 * Math.Sign(Min);
                double dSec = Sec / 3600;
                Numeric = dDeg + dMin + dSec;
                if (Deg != 0)
                    Numeric = Numeric * Math.Sign(Deg);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        #endregion // Convert Coordinate To Decimal

        #region Convert Decimal To Coordinate

        // Markus 8.7.2022 - Berechung der Koordinate in deg min sec
        public static bool ConvertDecimalToCoordinate(GeodeticPosition geodeticPosition, ref int LongDeg, ref int LongMin, ref double LongSec, ref int LatDeg, ref int LatMin, ref double LatSec)
        {
            bool OK = true;
            try
            {
                int PrefixLat = 1;
                convertFromNumericToDegMinSec(geodeticPosition.lat, ref PrefixLat, ref LatDeg, ref LatMin, ref LatSec);
                LatDeg = LatDeg * PrefixLat;
                int PrefixLong = 1;
                convertFromNumericToDegMinSec(geodeticPosition.lon, ref PrefixLong, ref LongDeg, ref LongMin, ref LongSec);
                LongDeg = LongDeg * PrefixLong;
            }
            catch (Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        // Markus 8.7.2022 - Berechung der Koordinate in deg min sec
        private static bool convertFromNumericToDegMinSec(double Numeric, ref int Prefix, ref int Deg, ref int Min, ref double Sec)
        {
            bool OK = true;
            try
            {
                //Prefix
                if (Numeric < 0) Prefix = -1;
                else Prefix = 1;
                // Deg
                Deg = (int)Numeric * Math.Sign(Numeric);
                // Min
                double MinD = Numeric * Math.Sign(Numeric) - Deg;
                MinD = MinD * 0.6;
                MinD = ((MinD * 100));
                Min = (int)MinD;
                // Sec
                double SecD = MinD - Min;
                SecD *= 60;
                decimal dSec = (decimal)SecD;
                dSec = Math.Round(dSec, 9);
                SecD = (double)dSec;
                if (SecD >= 60)
                {
                    SecD -= 60;
                    Min += 1;
                }
                decimal dSecD = (decimal)SecD;
                dSecD = System.Math.Round(dSecD, 9);
                SecD = (double)dSecD;
                Sec = SecD;
                Deg = System.Math.Abs(Deg);
            }
            catch (Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        #endregion

    }

    public class GeodeticPosition
    {
        ///Markus 8.7.22: Properties to ensure reset of conversion 
        private double _lon;
        private double _lat;
        private double _h;

        public double lon { get { return _lon; } set { _lon = value; _Converted = false; } }
        public double lat { get { return _lat; } set { _lat = value; _Converted = false; } }
        public double h { get { return _h; } set { _h = value; } }

        public GeodeticPosition(double longitude, double latitude, double altitude)
        {
            lon = longitude;
            lat = latitude;
            h = altitude;
        }

        ///Markus 8.7.22: Ausgabe 
        #region Ausgabe in deg min sec

        private int _LonDeg;
        private int _LonMin;
        private double _LonSec;
        private int _LatDeg;
        private int _LatMin;
        private double _LatSec;
        private bool _Converted = false;
        private void convertValuesToDegMinSec()
        {
            GeoCon.ConvertDecimalToCoordinate(this, ref _LonDeg, ref _LonMin, ref _LonSec, ref _LatDeg, ref _LatMin, ref _LatSec);
            _Converted = true;
        }
        public int lonDeg { get { if (!_Converted) { convertValuesToDegMinSec(); } return _LonDeg; } }
        public int lonMin { get { if (!_Converted) { convertValuesToDegMinSec(); } return _LonMin; } }
        public double lonSec { get { if (!_Converted) { convertValuesToDegMinSec(); } return _LonSec; } }

        public int latDeg { get { if (!_Converted) { convertValuesToDegMinSec(); } return _LatDeg; } }
        public int latMin { get { if (!_Converted) { convertValuesToDegMinSec(); } return _LatMin; } }
        public double latSec { get { if (!_Converted) { convertValuesToDegMinSec(); } return _LatSec; } }

        #endregion
    }
}
