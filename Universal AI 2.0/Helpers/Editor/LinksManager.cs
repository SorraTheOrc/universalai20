#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UniversalAI
{
    public class LinksManager : MonoBehaviour
    {
        [MenuItem("Tools/Universal AI/Support/Discord Server")]
        public static void Discord()
        {
            Application.OpenURL("https://discord.gg/bvY62aeQ3f");
        }
    
        [MenuItem("Tools/Universal AI/Support/E-Mail")]
        public static void Mail()
        {
            EditorUtility.DisplayDialog("Contact Us",
                "You can send your questions to 'darkingassets@gmail.com'. Our team will reply as soon as possible.",
                "Okay");
        }
    
        [MenuItem("Tools/Universal AI/Documentation/Online")]
        public static void OnlineDocs()
        {
            Application.OpenURL("https://darking-studios.gitbook.io/universal-ai-2.0/");
        }
    
        [MenuItem("Tools/Universal AI/Documentation/Offline")]
        public static void OfflineDocs()
        {
            if (EditorUtility.DisplayDialog("Warning!",
                    "Using offline documentation is not recommended, and errors may occur. Which one would you like to use?",
                    "Online", "Offline"))
            {
                Application.OpenURL("https://darking-studios.gitbook.io/universal-ai-2.0/");
            }
            else
            {
                Application.OpenURL(Application.dataPath + "/Universal AI 2.0/Resources/Documentation.pdf");
            }
        }
    }
}
#endif