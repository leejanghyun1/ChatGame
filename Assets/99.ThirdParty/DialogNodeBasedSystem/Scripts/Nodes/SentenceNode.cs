#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace cherrydev
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Nodes/Sentence Node", fileName = "New Sentence Node")]
    public class SentenceNode : Node
    {
        [SerializeField] private Sentence _sentence;

        [Space(10)] public Node ParentNode;
        public Node ChildNode;

        [Space(7)] [SerializeField] private bool _isExternalFunc;
        [SerializeField] private string _externalFunctionName;

        private string _externalButtonLabel;
        private const float LabelFieldSpace = 47f;
        private const float TextFieldWidth = 100f;
        private const float ExternalNodeHeight = 155f;

        [Space(10)]
        [Header("Sprite Path Handling")]
        [SerializeField] private string _spriteName; // 새로 추가된 스프라이트 경로
        private const string SpriteBasePath = "Assets/01.Resources/03.portrait/"; // 기본 경로
        private const string SpriteExtension = ".png"; // 파일 확장자

        /// <summary>
        /// Returning external function name
        /// </summary>
        public string GetExternalFunctionName() => _externalFunctionName;

        /// <summary>
        /// Returning sentence character name
        /// </summary>
        public string GetSentenceCharacterName() => _sentence.CharacterName;

        /// <summary>
        /// Setting sentence text
        /// </summary>
        public void SetSentenceText(string text) => _sentence.Text = text;

        /// <summary>
        /// Returning sentence text
        /// </summary>
        public string GetSentenceText() => _sentence.Text;

        /// <summary>
        /// Returning sentence character sprite
        /// </summary>
        public Sprite GetCharacterSprite() => _sentence.CharacterSprite;

        /// <summary>
        /// Returns the value of a isExternalFunc boolean field
        /// </summary>
        public bool IsExternalFunc() => _isExternalFunc;

#if UNITY_EDITOR
        /// <summary>
        /// Draw Sentence Node method
        /// </summary>
        public override void Draw(GUIStyle nodeStyle, GUIStyle labelStyle)
        {
            base.Draw(nodeStyle, labelStyle);

            GUILayout.BeginArea(Rect, nodeStyle);
            EditorGUILayout.LabelField("Sentence Node", labelStyle);

            DrawCharacterNameFieldHorizontal();
            DrawSentenceTextFieldHorizontal();
            DrawExternalFunctionTextField();
            DrawCharacterSpritePathField(); // 스프라이트 경로 입력 필드 추가

            if (GUILayout.Button(_externalButtonLabel))
            {
                _isExternalFunc = !_isExternalFunc;
            }

            GUILayout.EndArea();
        }

        /// <summary>
        /// Draw label and text fields for character name
        /// </summary>
        private void DrawCharacterNameFieldHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Name ", GUILayout.Width(LabelFieldSpace));
            _sentence.CharacterName = EditorGUILayout.TextField(_sentence.CharacterName, GUILayout.Width(TextFieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw label and text fields for sentence text
        /// </summary>
        private void DrawSentenceTextFieldHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Text ", GUILayout.Width(LabelFieldSpace));
            _sentence.Text = EditorGUILayout.TextField(_sentence.Text, GUILayout.Width(TextFieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw label and text fields for sprite path and load sprite
        /// </summary>
        private void DrawCharacterSpritePathField()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Sprite Path ", GUILayout.Width(LabelFieldSpace));
            _spriteName = EditorGUILayout.TextField(_spriteName, GUILayout.Width(TextFieldWidth));
            Rect.width = 230f;
            if (GUILayout.Button("Load"))
            {
                LoadSpriteFromPath();
            }
            EditorGUILayout.EndHorizontal();

            // 스프라이트 미리보기와 크기 조정
            if (_sentence.CharacterSprite != null)
            {
                GUILayout.Label(_sentence.CharacterSprite.texture, GUILayout.Width(100), GUILayout.Height(100));
                Rect.height = Mathf.Max(Rect.height, 270f); // 노드 크기를 동적으로 확장
            }
        }

        /// <summary>
        /// Load sprite from the given path
        /// </summary>
        private void LoadSpriteFromPath()
        {
            // 입력된 파일 이름을 기반으로 전체 경로 생성
            string fullPath = $"{SpriteBasePath}{_spriteName}{SpriteExtension}";

            if (string.IsNullOrEmpty(fullPath))
            {
                Debug.LogWarning("Sprite path is empty.");
                return;
            }

#if UNITY_EDITOR
            // Load asset using the path
            Sprite loadedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(fullPath);
            if (loadedSprite != null)
            {
                _sentence.CharacterSprite = loadedSprite;
                Debug.Log($"Sprite loaded successfully from: {fullPath}");
            }
            else
            {
                Debug.LogError($"No sprite found at path: {fullPath}");
            }
#else
            Debug.LogError("Sprite loading is supported only in the Unity Editor.");
#endif
        }

        /// <summary>
        /// Draw external function text field
        /// </summary>
        private void DrawExternalFunctionTextField()
        {
            if (_isExternalFunc)
            {
                _externalButtonLabel = "Remove external func";

                EditorGUILayout.BeginHorizontal();
                Rect.height = ExternalNodeHeight;
                EditorGUILayout.LabelField($"Func Name ", GUILayout.Width(LabelFieldSpace));
                _externalFunctionName = EditorGUILayout.TextField(_externalFunctionName,
                    GUILayout.Width(TextFieldWidth));
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                _externalButtonLabel = "Add external func";
                Rect.height = StandardHeight;
            }
        }

        /// <summary>
        /// Checking node size
        /// </summary>
        public void CheckNodeSize(float width, float height)
        {
            Rect.width = width;

            if (StandardHeight == 0)
            {
                StandardHeight = height;
            }

            if (_isExternalFunc)
                Rect.height = ExternalNodeHeight;
            else
                Rect.height = StandardHeight;
        }

#endif
    }
}