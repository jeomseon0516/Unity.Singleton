using Jeomseon.Unity.Singleton;
using NUnit.Framework;
using UnityEngine;

namespace Jeomseon.Tests
{
    public sealed class SceneSingletonTests
    {
        private sealed class TestSceneSingleton : SingletonScene<TestSceneSingleton>
        {
            public int InitializeCount { get; private set; }

            protected override void Init()
            {
                InitializeCount++;
            }
        }

        [TearDown]
        public void TearDown()
        {
            foreach (TestSceneSingleton instance in Resources.FindObjectsOfTypeAll<TestSceneSingleton>())
            {
                if (instance)
                {
                    Object.DestroyImmediate(instance.gameObject);
                }
            }
        }

        [Test]
        public void Instance_CreatesAndInitializesOneSceneObject()
        {
            TestSceneSingleton first = TestSceneSingleton.Instance;
            TestSceneSingleton second = TestSceneSingleton.Instance;

            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.SameAs(first));
            Assert.That(first.InitializeCount, Is.EqualTo(1));
            Assert.That(first.gameObject.name, Is.EqualTo(nameof(TestSceneSingleton)));
        }
    }
}
