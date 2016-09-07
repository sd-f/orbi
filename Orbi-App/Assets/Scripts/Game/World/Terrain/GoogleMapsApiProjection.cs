using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameController
{
    public class GoogleMapsApiProjection
    {
        private readonly int TILE_SIZE = 256;
        private static readonly int ZOOM = WorldAdapter.ZOOM;
        private PointF _pixelOrigin;
        private double _pixelsPerLonDegree;
        private double _pixelsPerLonRadian;
        private int NUM_TILES = (int)Math.Pow(2, ZOOM) * 2;

        public GoogleMapsApiProjection()
        {
            this._pixelOrigin = new PointF(0d, 0d); // new PointF(TILE_SIZE/2d, TILE_SIZE/2d);
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

        public PointF fromLatLngToPoint(double lat, double lng)
        {
            PointF point = new PointF(0d, 0d);

            point.x = lng * _pixelsPerLonDegree;

            // Truncating to 0.9999 effectively limits latitude to 89.189. This is
            // about a third of a tile past the edge of the world tile.
            //double siny = bound(Math.Sin(degreesToRadians(lat)), -0.9999d, 0.9999d);
            double siny = Math.Sin(degreesToRadians(lat));
            point.y = - ((.5 * Math.Log((1 + siny) / (1 - siny)) * _pixelsPerLonRadian));
            //point.y = 0.5 * Math.Log((1 + siny) / (1 - siny)) * -_pixelsPerLonRadian;

            return point;
        }

        public PointF toReal(PointF pixelPoint)
        {
            PointF point = new PointF(pixelPoint.x, pixelPoint.y);
            var lng = point.x / 256 * 360 - 180;
            var n = Math.PI - 2 * Math.PI * point.y / 256;
            var lat = (180 / Math.PI * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n))));
            return new PointF(lat, lng);
        }

        public PointF fromPointToLatLng(PointF pixelPoint)
        {
            PointF point = new PointF(pixelPoint.x / NUM_TILES, pixelPoint.y / NUM_TILES);
            double lng = point.x / _pixelsPerLonDegree;
            double latRadians = point.y / _pixelsPerLonRadian;
            //double lat = radiansToDegrees(2d * Math.Atan(Math.Exp(latRadians)) - Math.PI / 2d);
            double lat = (radiansToDegrees(Math.Atan(Math.Sinh(latRadians))));
            return new PointF(lat * NUM_TILES, lng * NUM_TILES);
        }


        public PointF WorldToTilePos(double lon, double lat)
        {
            PointF p = new PointF();
            p.x = ((lon + 180.0) / 360.0 * (1 << ZOOM));
            p.y = (((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
                1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << ZOOM)));

            return p;
        }

        public PointF TileToWorldPos(double tile_x, double tile_y)
        {
            PointF p = new PointF();
            // pi - (2pi * x) / tiles
            double n = Math.PI - ((2.0d * Math.PI * tile_y) / (Math.Pow(2.0d, ZOOM) * 256d));

            p.x = ((tile_x / (Math.Pow(2.0d, ZOOM)*256d) * 360.0d));
            p.y = (180.0d / Math.PI * Math.Atan(Math.Sinh(n)));

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
    }
}
