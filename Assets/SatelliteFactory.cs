using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

public class SatelliteFactory : MonoBehaviour
{
    static int lineIndex = 0;
    static List<Vector3> satellitePoints = new List<Vector3>();
    static GameObject satelliteInstance;
    public static void CreateSatellite(string filePath)
    {
        foreach (var line in File.ReadLines(filePath))
        {
            if (!line.Contains("UTCGregorian") && lineIndex < 361)
            {
                UnityEngine.Debug.Log("Line Index: " + lineIndex);
                string[] trimmedLine = Regex.Replace(line, @"\s+", " ").Split(' ');
                UnityEngine.Debug.Log("Line: " + line);
                float x = float.Parse(trimmedLine[4], CultureInfo.InvariantCulture.NumberFormat) / 5000000;
                float y = float.Parse(trimmedLine[5], CultureInfo.InvariantCulture.NumberFormat) / 5000000;
                float z = float.Parse(trimmedLine[6], CultureInfo.InvariantCulture.NumberFormat) / 5000000;

                if (lineIndex == 0)
                {
                    UnityEngine.Debug.Log("Prueba Line Index");

                    GameObject satellitePrefab = Resources.Load<GameObject>("magellan/maggellan_ex");

                    if (satellitePrefab != null)
                    {
                        UnityEngine.Debug.Log("COMPROBACION INDEX: " + x);
                        satelliteInstance = Instantiate(satellitePrefab, new Vector3(x, y, z), Quaternion.Euler(90.0f, 0, 0));
                        satelliteInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                        SatelliteTrajectory trajectoryComponent = satelliteInstance.AddComponent<SatelliteTrajectory>();
                        LineRenderer orbitTrajectory = satelliteInstance.AddComponent<LineRenderer>();
                        orbitTrajectory.enabled = false;
                    }
                    else
                    {
                        UnityEngine.Debug.Log("NULL PREFAB");
                    }
                }
                else
                {
                    Vector3 satellitePoint = new Vector3(x, y, z);
                    satellitePoints.Add(satellitePoint);
                }
                lineIndex++;
            }
        }

        SatelliteTrajectory satelliteTrajectory = satelliteInstance.AddComponent<SatelliteTrajectory>();
        satelliteTrajectory.GetTrajectories(satellitePoints);
    }
}
