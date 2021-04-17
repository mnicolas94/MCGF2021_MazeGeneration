﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class TilemapExtension
    {
        public static IEnumerable<Vector3Int> GetTilePositions(this Tilemap tm)
        {
            var bounds = tm.cellBounds;
            for (int i = bounds.xMin; i < bounds.xMax; i++)
            {
                for (int j = bounds.yMin; j < bounds.yMax; j++)
                {
                    for (int k = bounds.zMin; k < bounds.zMax; k++)
                    {
                        Vector3Int tilepos = new Vector3Int(i, j, k);
                        if (tm.HasTile(tilepos))
                        {
                            yield return tilepos;
                        }
                    }
                }    
            }
        }
    
        public static IEnumerable<Vector3> GetTileWorldPositions(this Tilemap tm)
        {
            var tilespos = tm.GetTilePositions();
            foreach (var tp in tilespos)
            {
                yield return tm.GetCellCenterWorld(tp);
            }
        }
    
        public static IEnumerable<TileBase> GetTiles(this Tilemap tm)
        {
            var tilespos = tm.GetTilePositions();
            foreach (var tp in tilespos)
            {
                yield return tm.GetTile(tp);
            }
        }

        /// <summary>
        /// Obtener las posiciones del borde del tilemap. Funciona correctamente solo con tilemaps rectangulares.
        /// </summary>
        /// <returns></returns>
        public static List<Vector3Int> GetEdgePositions(this Tilemap tm)
        {
            List<Vector3Int> edgePos = new List<Vector3Int>();
            var bounds = tm.cellBounds;
            for (int i = bounds.xMin; i < bounds.xMax; i++)  // iterar las columnas
            {
                for (int k = bounds.zMin; k < bounds.zMax; k++)
                {
                    int minRow = bounds.yMax;
                    int maxRow = bounds.yMin;
                    bool existsTile = false;
                    Vector3Int vtemp = Vector3Int.zero;
                    for (int j = bounds.yMin; j < bounds.yMax; j++)
                    {
                        vtemp.x = i; vtemp.y = j; vtemp.z = k;
                        if (tm.HasTile(vtemp))
                        {
                            maxRow = j;
                            if (j < minRow)
                                minRow = j;
                            existsTile = true;
                        }
                    }
                    if (existsTile)
                    {
                        edgePos.Add(new Vector3Int(i, minRow - 1, k));
                        edgePos.Add(new Vector3Int(i, maxRow + 1, k));
                    }
                }
            }
        
            for (int j = bounds.yMin; j < bounds.yMax; j++)  // iterar las filas
            {
                for (int k = bounds.zMin; k < bounds.zMax; k++)
                {
                    int minCol = bounds.xMax;
                    int maxCol = bounds.xMin;
                    bool existsTile = false;
                    Vector3Int vtemp = Vector3Int.zero;
                    for (int i = bounds.xMin; i < bounds.xMax; i++)
                    {
                        vtemp.x = i; vtemp.y = j; vtemp.z = k;
                        if (tm.HasTile(vtemp))
                        {
                            maxCol = i;
                            if (i < minCol)
                                minCol = i;
                            existsTile = true;
                        }
                    }
                    if (existsTile)
                    {
                        edgePos.Add(new Vector3Int(minCol - 1, j, k));
                        edgePos.Add(new Vector3Int(maxCol + 1, j, k));
                    }
                }
            }

            return edgePos;
        }

        /// <summary>
        /// Devuelve los verdaderos límites de un Tilemap. El parámetro Tilemap.bounds tiene en cuenta posiciones de casillas
        /// eliminadas por lo tanto en ocasiones los límites no son precisos.
        /// Este método se puede optimizar para no tener que iterar por todos los elementos.
        /// </summary>
        /// <param name="tm"></param>
        /// <returns></returns>
        public static BoundsInt RealCellBounds(this Tilemap tm)
        {
            var bounds = tm.cellBounds;
            int minx = bounds.xMax;
            int miny = bounds.yMax;
            int minz = bounds.zMax;
            int maxx = bounds.xMin;
            int maxy = bounds.yMin;
            int maxz = bounds.zMin;
            for (int i = bounds.xMin; i < bounds.xMax; i++)
            {
                for (int j = bounds.yMin; j < bounds.yMax; j++)
                {
                    for (int k = bounds.zMin; k < bounds.zMax; k++)
                    {
                        Vector3Int tilepos = new Vector3Int(i, j, k);
                        if (tm.HasTile(tilepos))
                        {
                            if (i < minx) minx = i;
                            if (i > maxx) maxx = i;
                            if (j < miny) miny = j;
                            if (j > maxy) maxy = j;
                            if (k < minz) minz = k;
                            if (k > maxz) maxz = k;
                        }
                    }
                }    
            }
            bounds.xMin = minx;
            bounds.xMax = maxx + 1;
            bounds.yMin = miny;
            bounds.yMax = maxy + 1;
            bounds.zMin = minz;
            bounds.zMax = maxz + 1;
        
            return bounds;
        }

        public static Bounds WorldBounds(this Tilemap tm)
        {
            var cellBounds = tm.RealCellBounds();
            var tmPos = tm.transform.position;
            Vector3 min = Vector3.Scale(cellBounds.min, tm.cellSize) + tmPos;
            Vector3 max = Vector3.Scale(cellBounds.max, tm.cellSize) + tmPos;
            var bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }
    }
}