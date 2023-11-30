﻿using System.Linq;
using Dialogue.RunTime;
using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Editor.Graph
{
    public class StoryGraph : EditorWindow
    {
        private string _fileName = "New Narrative";

        private StoryGraphView _graphView;
        private DialogueContainer _dialogueContainer;
        private MiniMap miniMap;

        [MenuItem("Graph/Narrative Graph")]
        public static void CreateGraphViewWindow()
        {
            var window = GetWindow<StoryGraph>();
            window.titleContent = new GUIContent("Narrative Graph");
        }

        private void ConstructGraphView()
        {
            _graphView = new StoryGraphView(this)
            {
                name = "Narrative Graph",
            };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) {text = "Save Data"});
            toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});
            
            var refreshMinimapPositionButton = new Button(RefreshMinimapPosition);
            refreshMinimapPositionButton.text = "Refresh Minimap";
            toolbar.Add(refreshMinimapPositionButton);
            
            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (!string.IsNullOrEmpty(_fileName))
            {
                var saveUtility = GraphSaveUtility.GetInstance(_graphView);
                if (save)
                    saveUtility.SaveGraph(_fileName);
                else
                    saveUtility.LoadNarrative(_fileName);
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid File name", "Please Enter a valid filename", "OK");
            }
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMinimap();
            GenerateBlackBoard();
        }

        private void GenerateMinimap()
        {
            miniMap = new MiniMap
            {
                anchored = true
            };
            _graphView.Add(miniMap);
            RefreshMinimapPosition();
        }

        public void RefreshMinimapPosition()
        {
            miniMap.SetPosition(new Rect(position.width - 210, 30, 200, 140));
        }

        private void GenerateBlackBoard()
        {
            var blackboard = new Blackboard(_graphView);
            blackboard.Add(new BlackboardSection {title = "Exposed Variables"});
            blackboard.addItemRequested = _blackboard =>
            {
                _graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
            };
            blackboard.editTextRequested = (_blackboard, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField) element).text;
                if (_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
                        "OK");
                    return;
                }

                var targetIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                _graphView.ExposedProperties[targetIndex].PropertyName = newValue;
                ((BlackboardField) element).text = newValue;
            };
            blackboard.SetPosition(new Rect(10,30,200,300));
            _graphView.Add(blackboard);
            _graphView.Blackboard = blackboard;
            _graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstancePlaceHolder(), false);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }
    }
}