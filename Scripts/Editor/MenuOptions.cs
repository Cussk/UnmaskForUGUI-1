using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Oultrox.UIExtensions.Editors
{
    internal class MenuOptions_UIUnmask
    {
        [MenuItem("GameObject/UI/Unmask/Tutorial Button")]
        private static void CreateTutorialButton2(MenuCommand menuCommand)
        {
#if UNITY_2021_2_OR_NEWER
            const string menuItemName = "GameObject/UI/Legacy/Button";
#else
            const string menuItemName = "GameObject/UI/Button";
#endif
            EditorApplication.ExecuteMenuItem(menuItemName);
            var button = Selection.activeGameObject.GetComponent<Button>();
            button.name = "Tutorial Button";

            var unmaskedPanel = CreateUnmaskedPanel(AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd"), Image.Type.Sliced);
            var unmask = unmaskedPanel.GetComponentInChildren<Unmask>();
            unmask.SameSceneFitTargetName = button.name;
            unmask.fitOnLateUpdate = true;

            Selection.activeGameObject = button.gameObject;
        }

        [MenuItem("GameObject/UI/Unmask/Iris Shot")]
        private static void CreateTransition(MenuCommand menuCommand)
        {
            var unmaskedPanel = CreateUnmaskedPanel(AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd"), Image.Type.Simple);
            unmaskedPanel.name = "Iris Shot";
            Selection.activeGameObject = unmaskedPanel;
        }

        [MenuItem("GameObject/UI/Unmask/Unmasked Panel")]
        private static GameObject CreateUnmaskedPanel(MenuCommand menuCommand)
        {
            return CreateUnmaskedPanel(AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd"), Image.Type.Sliced);
        }

        private static GameObject CreateUnmaskedPanel(Sprite unmaskSprite, Image.Type spriteType)
        {
            EditorApplication.ExecuteMenuItem("GameObject/UI/Panel");
            var mask = Selection.activeGameObject.AddComponent<Mask>();
            mask.showMaskGraphic = false;
            mask.name = "Unmasked Panel";
            mask.GetComponent<Image>().sprite = null;

            EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
            var unmask = Selection.activeGameObject.AddComponent<Unmask>();
            unmask.name = "Unmask";
            unmask.transform.SetParent(mask.transform);
            unmask.GetComponent<Image>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

            var unmaskRaycastFilter = mask.gameObject.AddComponent<UnmaskRaycastFilter>();
            unmaskRaycastFilter.targetUnmasks = new Unmask[1];
            unmaskRaycastFilter.targetUnmasks[0] = unmask;

            var image = unmask.GetComponent<Image>();
            image.sprite = unmaskSprite;
            image.type = spriteType;

            EditorApplication.ExecuteMenuItem("GameObject/UI/Panel");
            var screen = Selection.activeGameObject.GetComponent<Image>();
            screen.name = "Screen";
            screen.sprite = null;
            screen.color = new Color(0, 0, 0, 0.8f);
            screen.transform.SetParent(mask.transform);

            return mask.gameObject;
        }
    }
}