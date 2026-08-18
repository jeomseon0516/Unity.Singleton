using System.Collections;
using Jeomseon.Unity.Singleton;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Jeomseon.Tests
{
    public sealed class SingletonPlayModeTests
    {
        private sealed class TestSingleton : Singleton<TestSingleton>
        {
            public static int DisposeCount { get; private set; }

            public int AwakeCount { get; private set; }
            public int InitializeCount { get; private set; }

            public override SingletonLifetime Lifetime => SingletonLifetime.Scene;

            public static void ResetProbe()
            {
                DisposeCount = 0;
            }

            public void Awake()
            {
                AwakeCount++;
            }

            protected override void OnSingletonInitialize()
            {
                InitializeCount++;
            }

            protected override void OnSingletonDispose()
            {
                DisposeCount++;
            }
        }

        private sealed class QuittingSingleton : Singleton<QuittingSingleton>
        {
            protected override void OnSingletonInitialize() { }
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            foreach (TestSingleton instance in Resources.FindObjectsOfTypeAll<TestSingleton>())
            {
                if (instance)
                {
                    Object.Destroy(instance.gameObject);
                }
            }

            foreach (QuittingSingleton instance in Resources.FindObjectsOfTypeAll<QuittingSingleton>())
            {
                if (instance)
                {
                    Object.Destroy(instance.gameObject);
                }
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator Instance_CreatesAndInitializesOneSceneLifetimeObject()
        {
            TestSingleton.ResetProbe();
            TestSingleton first = TestSingleton.Instance;
            TestSingleton second = TestSingleton.Instance;

            yield return null;

            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.SameAs(first));
            Assert.That(first.InitializeCount, Is.EqualTo(1));
            Assert.That(TestSingleton.HasInstance, Is.True);
            Assert.That(first.Lifetime, Is.EqualTo(SingletonLifetime.Scene));
            Assert.That(first.IsInitialized, Is.True);
        }

        [UnityTest]
        public IEnumerator DerivedAwakeAndRelayLifecycle_BothRunInPlayMode()
        {
            TestSingleton.ResetProbe();
            var gameObject = new GameObject(nameof(TestSingleton));
            TestSingleton instance = gameObject.AddComponent<TestSingleton>();

            yield return null;

            Assert.That(instance.AwakeCount, Is.EqualTo(1));
            Assert.That(instance.InitializeCount, Is.EqualTo(1));
            Assert.That(instance.IsInitialized, Is.True);
            Assert.That(instance.GetComponent<SingletonLifecycleRelay>(), Is.Not.Null);

            Object.Destroy(gameObject);
            yield return null;

            Assert.That(TestSingleton.DisposeCount, Is.EqualTo(1));
            Assert.That(TestSingleton.HasInstance, Is.False);
        }

        [UnityTest]
        public IEnumerator Destroy_DisposesInitializedHost()
        {
            TestSingleton.ResetProbe();
            TestSingleton instance = TestSingleton.Instance;

            yield return null;

            Object.Destroy(instance.gameObject);
            yield return null;

            Assert.That(TestSingleton.DisposeCount, Is.EqualTo(1));
            Assert.That(TestSingleton.HasInstance, Is.False);
        }

        [UnityTest]
        public IEnumerator ApplicationQuit_PreventsInstanceAccess()
        {
            QuittingSingleton instance = QuittingSingleton.Instance;

            yield return null;

            instance.SendMessage("OnApplicationQuit");

            Assert.That(QuittingSingleton.Instance, Is.Null);
        }

        [UnityTest]
        public IEnumerator SceneLifetime_UnloadDisposesHostAndNextAccessCreatesNewInstance()
        {
            TestSingleton.ResetProbe();
            Scene ownerScene = SceneManager.CreateScene("Singleton scene lifetime owner");
            var ownerObject = new GameObject("Scene singleton");
            SceneManager.MoveGameObjectToScene(ownerObject, ownerScene);
            TestSingleton first = ownerObject.AddComponent<TestSingleton>();
            yield return null;

            Assert.That(TestSingleton.Instance, Is.SameAs(first));

            yield return SceneManager.UnloadSceneAsync(ownerScene);

            Assert.That(TestSingleton.DisposeCount, Is.EqualTo(1));
            Assert.That(TestSingleton.HasInstance, Is.False);

            TestSingleton second = TestSingleton.Instance;
            yield return null;

            Assert.That(second, Is.Not.Null);
            Assert.That(second, Is.Not.SameAs(first));
            Assert.That(second.gameObject.scene, Is.EqualTo(SceneManager.GetActiveScene()));
        }

        [UnityTest]
        public IEnumerator AdditiveDuplicate_KeepsEarlierSceneInstanceAndRemovesOnlyDuplicateComponent()
        {
            TestSingleton.ResetProbe();
            Scene firstScene = SceneManager.CreateScene("Singleton additive first");
            Scene secondScene = SceneManager.CreateScene("Singleton additive second");
            var firstObject = new GameObject("Preferred singleton");
            var secondObject = new GameObject("Duplicate singleton");
            firstObject.SetActive(false);
            secondObject.SetActive(false);
            SceneManager.MoveGameObjectToScene(firstObject, firstScene);
            SceneManager.MoveGameObjectToScene(secondObject, secondScene);
            TestSingleton first = firstObject.AddComponent<TestSingleton>();
            TestSingleton second = secondObject.AddComponent<TestSingleton>();

            LogAssert.Expect(
                LogType.Warning,
                new System.Text.RegularExpressions.Regex(
                    "Duplicate TestSingleton instances were found.*Keeping.*Preferred singleton"));

            secondObject.SetActive(true);
            yield return null;

            Assert.That(TestSingleton.Instance, Is.SameAs(first));
            Assert.That(first.IsInitialized, Is.True);
            Assert.That(second == null, Is.True);
            Assert.That(secondObject, Is.Not.Null);

            Object.Destroy(secondObject);
            yield return SceneManager.UnloadSceneAsync(secondScene);
            yield return SceneManager.UnloadSceneAsync(firstScene);
        }
    }
}
