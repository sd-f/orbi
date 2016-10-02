using ClientModel;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameController
{
    public class Projection
    {
        private readonly int TILE_SIZE = 256;
        private static readonly int ZOOM = WorldAdapter.ZOOM;
        private double _pixelsPerLonDegree;
        private double _pixelsPerLonRadian;
        public static int NUM_TILES = (int)Math.Pow(2, ZOOM);

        public Projection()
        {
            this._pixelsPerLonDegree = TILE_SIZE / 360.0d;
            this._pixelsPerLonRadian = TILE_SIZE / (2d * Math.PI);
        }

        double bound(double val, double valMin, double valMax)
        {
            double res;
            res = Math.Max(val, valMin);
            res = Math.Min(res, valMax);
            return res;
        }

        double degreesToRadians(double deg)
        {
            return deg * (Math.PI / 180d);
        }

        double radiansToDegrees(double rad)
        {
            return rad / (Math.PI / 180d);
        }

        public Position fromLatLngToPoint(GeoPosition original)
        {
            // latitude  --> z 
            // longitude --> x
            // altitude  --> y
            Position point = new Position(original.longitude, original.altitude, original.latitude);
            point.x = point.x * _pixelsPerLonDegree;
            // Truncating to 0.9999 effectively limits latitude to 89.189. This is
            // about a third of a tile past the edge of the world tile.
            //double siny = bound(Math.Sin(degreesToRadians(lat)), -0.9999d, 0.9999d);
            double siny = Math.Sin(degreesToRadians(point.z));
            point.z = (.5d * Math.Log((1d + siny) / (1d - siny)) * _pixelsPerLonRadian);
            return point;
        }

        public GeoPosition fromPointToLatLng(Position original)
        {
            // z --> latitude
            // x --> longitude
            // y --> altitude
            GeoPosition point = new GeoPosition(original.z, original.x, original.y);
            point.longitude = point.longitude / _pixelsPerLonDegree;
            double latRadians = point.latitude / _pixelsPerLonRadian;
            point.latitude = radiansToDegrees(2d * Math.Atan(Math.Exp(latRadians)) - Math.PI / 2d);
            return point;
        }

        public PointF WorldToTilePos(double lon, double lat)
        {
            PointF p = new PointF();
            p.x = (float)((lon + 180.0) / 360.0 * (1 << ZOOM));
            p.y = (float)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
                1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << ZOOM));

            return p;
        }

        public PointF TileToWorldPos(double tile_x, double tile_y)
        {
            PointF p = new PointF();
            double n = Math.PI - ((2.0 * Math.PI * tile_y) / Math.Pow(2.0, ZOOM));

            p.x = (float)((tile_x / Math.Pow(2.0, ZOOM) * 360.0) - 180.0);
            p.y = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            return p;
        }

    }

    


    public class PointF
    {
        public double x;
        public double y;

        public PointF()
        {

        }

        public PointF(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "PointF(" + x + "," + y + ")";
        }
    }
}
