using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;

[CustomEditor(typeof(Readmeo))]
[InitializeOnLoad]
public class ReadmeoEditor : Editor
{
    static string s_ShowedReadmeoSessionStateName = "ReadmeoEditor.showedReadmeo";
    
    static string s_ReadmeoSourceDirectory = "Assets/TutorialInfo";

    const float k_Space = 16f;

    static ReadmeoEditor()
    {
        EditorApplication.delayCall += SelectReadmeoAutomatically;
    }

    static void RemoveTutorial()
    {
        if (EditorUtility.DisplayDialog("Remove Readmeo Assets",
            
            $"All contents under {s_ReadmeoSourceDirectory} will be removed, are you sure you want to proceed?",
            "Proceed",
            "Cancel"))
        {
            if (Directory.Exists(s_ReadmeoSourceDirectory))
            {
                FileUtil.DeleteFileOrDirectory(s_ReadmeoSourceDirectory);
                FileUtil.DeleteFileOrDirectory(s_ReadmeoSourceDirectory + ".meta");
            }
            else
            {
                Debug.Log($"Could not find the Readmeo folder at {s_ReadmeoSourceDirectory}");
            }

            var ReadmeoAsset = SelectReadmeo();
            if (ReadmeoAsset != null)
            {
                var path = AssetDatabase.GetAssetPath(ReadmeoAsset);
                FileUtil.DeleteFileOrDirectory(path + ".meta");
                FileUtil.DeleteFileOrDirectory(path);
            }

            AssetDatabase.Refresh();
        }
    }

    static void SelectReadmeoAutomatically()
    {
        if (!SessionState.GetBool(s_ShowedReadmeoSessionStateName, false))
        {
            var Readmeo = SelectReadmeo();
            SessionState.SetBool(s_ShowedReadmeoSessionStateName, true);

            if (Readmeo && !Readmeo.loadedLayout)
            {
                LoadLayout();
                Readmeo.loadedLayout = true;
            }
        }
    }

    static void LoadLayout()
    {
        var assembly = typeof(EditorApplication).Assembly;
        var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
        var method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
        method.Invoke(null, new object[] { Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false });
    }

    static Readmeo SelectReadmeo()
    {
        var ids = AssetDatabase.FindAssets("Readmeo t:Readmeo");
        if (ids.Length == 1)
        {
            var ReadmeoObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));

            Selection.objects = new UnityEngine.Object[] { ReadmeoObject };

            return (Readmeo)ReadmeoObject;
        }
        else
        {
            Debug.Log("Couldn't find a Readmeo");
            return null;
        }
    }

    protected override void OnHeaderGUI()
    {
        var Readmeo = (Readmeo)target;
        Init();

        var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

        GUILayout.BeginHorizontal("In BigTitle");
        {
            if (Readmeo.icon != null)
            {
                GUILayout.Space(k_Space);
                GUILayout.Label(Readmeo.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
            }
            GUILayout.Space(k_Space);
            GUILayout.BeginVertical();
            {

                GUILayout.FlexibleSpace();
                GUILayout.Label(Readmeo.title, TitleStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
        var Readmeo = (Readmeo)target;
        Init();

        foreach (var section in Readmeo.sections)
        {
            if (!string.IsNullOrEmpty(section.heading))
            {
                GUILayout.Label(section.heading, HeadingStyle);
            }

            if (!string.IsNullOrEmpty(section.text))
            {
                GUILayout.Label(section.text, BodyStyle);
            }

            if (!string.IsNullOrEmpty(section.linkText))
            {
                if (LinkLabel(new GUIContent(section.linkText)))
                {
                    Application.OpenURL(section.url);
                }
            }

            GUILayout.Space(k_Space);
        }

        if (GUILayout.Button("Remove Readmeo Assets", ButtonStyle))
        {
            RemoveTutorial();
        }
    }

    bool m_Initialized;

    GUIStyle LinkStyle
    {
        get { return m_LinkStyle; }
    }

    [SerializeField]
    GUIStyle m_LinkStyle;

    GUIStyle TitleStyle
    {
        get { return m_TitleStyle; }
    }

    [SerializeField]
    GUIStyle m_TitleStyle;

    GUIStyle HeadingStyle
    {
        get { return m_HeadingStyle; }
    }

    [SerializeField]
    GUIStyle m_HeadingStyle;

    GUIStyle BodyStyle
    {
        get { return m_BodyStyle; }
    }

    [SerializeField]
    GUIStyle m_BodyStyle;

    GUIStyle ButtonStyle
    {
        get { return m_ButtonStyle; }
    }

    [SerializeField]
    GUIStyle m_ButtonStyle;

    void Init()
    {
        if (m_Initialized)
            return;
        m_BodyStyle = new GUIStyle(EditorStyles.label);
        m_BodyStyle.wordWrap = true;
        m_BodyStyle.fontSize = 14;
        m_BodyStyle.richText = true;

        m_TitleStyle = new GUIStyle(m_BodyStyle);
        m_TitleStyle.fontSize = 26;

        m_HeadingStyle = new GUIStyle(m_BodyStyle);
        m_HeadingStyle.fontStyle = FontStyle.Bold;
        m_HeadingStyle.fontSize = 18;

        m_LinkStyle = new GUIStyle(m_BodyStyle);
        m_LinkStyle.wordWrap = false;

        // Match selection color which works nicely for both light and dark skins
        m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
        m_LinkStyle.stretchWidth = false;

        m_ButtonStyle = new GUIStyle(EditorStyles.miniButton);
        m_ButtonStyle.fontStyle = FontStyle.Bold;

        m_Initialized = true;
    }

    bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
    {
        var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

        Handles.BeginGUI();
        Handles.color = LinkStyle.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

        return GUI.Button(position, label, LinkStyle);
    }
}
