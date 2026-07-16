#if UNITY_IOS
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

public class ProvideExportCompliance
{
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        // Performs any post build processes that we need done
        if (buildTarget == BuildTarget.iOS)
        {
            // PList modifications
            {
                // Get plist
                string plistPath = pathToBuiltProject + "/Info.plist";
                var plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));

                // Get root
                var rootDict = plist.root;

                // Add export compliance for TestFlight builds
                var buildKeyExportCompliance = "ITSAppUsesNonExemptEncryption";
                rootDict.SetString(buildKeyExportCompliance, "false");

                // Write to file
                File.WriteAllText(plistPath, plist.WriteToString());
            }
            /*
                        // xCode workspace modifications
                        {
                            // Get xCode Project
                            string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
                            PBXProject project = new PBXProject();
                            project.ReadFromFile(projectPath);
                            string targetGuid = project.GetUnityMainTargetGuid();
            
                            // Add AdSupport(AdMob) & CloudKit(iCloud) framework
                            project.AddFrameworkToProject(targetGuid, "iAd.framework", false);
                            project.AddFrameworkToProject(targetGuid, "CloudKit.framework", false);
                            project.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
                            project.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
                            project.AddFrameworkToProject(targetGuid, "AdServices.framework", false);
            
                            // Disable bitcode
                            project.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            
                            // Update xCode project to use the iCloud entitlements file and iCloud/AdSupport frameworks
                            string projectString = project.WriteToString();
            
                            // Save the xCode project file
                            File.WriteAllText(projectPath, projectString);
                        }*/
        }
    }
}
#endif
