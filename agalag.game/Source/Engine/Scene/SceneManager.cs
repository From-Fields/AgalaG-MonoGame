using System.Collections.Generic;
using agalag.engine.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public class SceneManager: Singleton<SceneManager>, iParentObject
    {
        //Attributes
        private Scene _defaultScene;
        private static Scene _currentScene;
        private Queue<Scene> _sceneQueue = new Queue<Scene>();
        private FixedUpdater _updater = new FixedUpdater();

        //Methods
        public void SwitchScene(Scene newScene, ContentManager content)
        {
            if(newScene == null)
                throw new System.ArgumentNullException("Switched Scene Cannot be Null");
            
            if(_currentScene != null)
                ClearScene(_currentScene, content);

            _currentScene = newScene;
            InitializeScene(newScene, content);

            if(!newScene.isLoaded)
                throw new System.Exception("Scene failed to Load");
            if(!newScene.isInitialized)
                throw new System.Exception("Scene failed to Initialize");
        }
        public void SwitchToDefaultScene(ContentManager content) => SwitchScene(_defaultScene, content);

        public void SetDefaultScene(Scene scene) 
        {
            if(scene == null) 
                throw new System.ArgumentNullException("Default Scene Cannot be Null");

            _defaultScene = scene;
        }

        private void ClearScene(Scene scene, ContentManager content) 
        {
            if(!scene.isLoaded || !scene.isInitialized)
                return;

            _currentScene.Clear();
            _currentScene.UnloadContent(content);
        }
        private void InitializeScene(Scene scene, ContentManager content) 
        {
            scene.LoadContent(content);
            scene.Initialize();
        }
        private void ConfigureFixedUpdate(float fixedUpdateDelta, float maxFrameTime)
        {
            this._updater = new FixedUpdater(fixedUpdateDelta, maxFrameTime);
        }

        # region HandlingSceneObjects
        public static void AddToMainScene(MonoEntity entity, Layer layer = Layer.Default)
        {
            _currentScene.AddElementToLayer(entity, layer);
        }

        public static void RemoveFromScene(MonoEntity entity)
        {
            _currentScene.RemoveElementFromLayer(entity);
        }

        #endregion

        #region Interface Implementation

        //iParent
        public void DrawChildren(SpriteBatch spriteBatch)
        {
            if(_currentScene != null)
            {
                _currentScene.Draw(spriteBatch);
                _currentScene.DrawChildren(spriteBatch);
            }
        }
        public void FixedUpdateChildren(GameTime gameTime, FixedFrameTime fixedFrameTime)
        {
            if(_currentScene != null)
            {
                _currentScene.FixedUpdate(gameTime, fixedFrameTime);
                _currentScene.FixedUpdateChildren(gameTime, fixedFrameTime);
            }
        }
        public void UpdateChildren(GameTime gameTime)
        {
            if(_currentScene != null)
            {
                _updater.ExecuteFixedUpdate(gameTime, (gT, ffT) => FixedUpdateChildren(gT, ffT));

                _currentScene.Update(gameTime);
                _currentScene.UpdateChildren(gameTime);
            }
        }

        #endregion
    }
}