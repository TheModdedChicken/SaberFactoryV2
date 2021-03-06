﻿using System.Linq;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Tags;
using HMUI;
using IPA.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRUIControls;

namespace SaberFactory.UI.Lib.BSML
{
    public class CustomScrollViewTag : BSMLTag
    {
        public override string[] Aliases => new[] { "this.scroll-view" };

        public override GameObject CreateObject(Transform parent)
        {
            TextPageScrollView textScrollView = MonoBehaviour.Instantiate(Resources.FindObjectsOfTypeAll<ReleaseInfoViewController>().First().GetField<TextPageScrollView, ReleaseInfoViewController>("_textPageScrollView"), parent);
            textScrollView.name = "BSMLScrollView";
            Button pageUpButton = textScrollView.GetField<Button, ScrollView>("_pageUpButton");
            Button pageDownButton = textScrollView.GetField<Button, ScrollView>("_pageDownButton");
            VerticalScrollIndicator verticalScrollIndicator = textScrollView.GetField<VerticalScrollIndicator, ScrollView>("_verticalScrollIndicator");

            RectTransform viewport = textScrollView.GetField<RectTransform, ScrollView>("_viewport");
            viewport.gameObject.AddComponent<VRGraphicRaycaster>().SetField("_physicsRaycaster", BeatSaberUI.PhysicsRaycasterWithCache);

            GameObject.Destroy(textScrollView.GetField<TextMeshProUGUI, TextPageScrollView>("_text").gameObject);
            GameObject gameObject = textScrollView.gameObject;
            MonoBehaviour.Destroy(textScrollView);
            gameObject.SetActive(false);

            BSMLScrollView scrollView = gameObject.AddComponent<BSMLScrollView>();
            scrollView.SetField<ScrollView, Button>("_pageUpButton", pageUpButton);
            scrollView.SetField<ScrollView, Button>("_pageDownButton", pageDownButton);
            scrollView.SetField<ScrollView, VerticalScrollIndicator>("_verticalScrollIndicator", verticalScrollIndicator);
            scrollView.SetField<ScrollView, RectTransform>("_viewport", viewport);

            viewport.anchorMin = new Vector2(0, 0);
            viewport.anchorMax = new Vector2(1, 1);

            GameObject parentObj = new GameObject();
            parentObj.name = "BSMLScrollViewContent";
            parentObj.transform.SetParent(viewport, false);

            ContentSizeFitter contentSizeFitter = parentObj.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup verticalLayout = parentObj.AddComponent<VerticalLayoutGroup>();
            verticalLayout.childForceExpandHeight = false;
            verticalLayout.childForceExpandWidth = false;
            verticalLayout.childControlHeight = true;
            verticalLayout.childControlWidth = true;
            verticalLayout.childAlignment = TextAnchor.UpperCenter;

            RectTransform rectTransform = parentObj.transform as RectTransform;

            parentObj.AddComponent<LayoutElement>();
            parentObj.AddComponent<ScrollViewContent>().scrollView = scrollView;

            GameObject child = new GameObject();
            child.name = "BSMLScrollViewContentContainer";
            child.transform.SetParent(rectTransform, false);

            VerticalLayoutGroup layoutGroup = child.AddComponent<VerticalLayoutGroup>();
            layoutGroup.childControlHeight = false;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childAlignment = TextAnchor.LowerCenter;
            layoutGroup.spacing = 0.5f;

            parentObj.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            child.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            child.AddComponent<LayoutElement>();
            ExternalComponents externalComponents = child.AddComponent<ExternalComponents>();
            externalComponents.components.Add(scrollView);
            externalComponents.components.Add(scrollView.transform);
            externalComponents.components.Add(gameObject.AddComponent<LayoutElement>());

            (child.transform as RectTransform).sizeDelta = new Vector2(0, -1);

            scrollView.SetField<ScrollView, RectTransform>("_contentRectTransform", parentObj.transform as RectTransform);
            gameObject.SetActive(true);

            Man(rectTransform);

            return child;
        }

        private async void Man(RectTransform rect)
        {
            await Task.Delay(50);
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.sizeDelta = new Vector2(0, 0);
            rect.pivot = new Vector2(0.5f, 1);
        }
    }
}