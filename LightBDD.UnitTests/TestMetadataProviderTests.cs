﻿using System.Linq;
using LightBDD.UnitTests.Helpers;
using NUnit.Framework;

namespace LightBDD.UnitTests
{
    [TestFixture]
    public class TestMetadataProviderTests
    {
        private TestMetadataProvider _subject;

        [ScenarioCategory("BaseC")]
        [FeatureDescription("BaseDescription")]
        class BaseClass
        {
            [ScenarioCategory("BaseMA")]
            public virtual void MethodA() { }
            [ScenarioCategory("BaseMB")]
            public virtual void MethodB() { }
        }

        [ScenarioCategory("DerivedC")]
        [FeatureDescription("DerivedDescription")]
        class DerivedClass : BaseClass
        {
            [ScenarioCategory("DerivedMB")]
            public override void MethodB()
            {
            }

            [ScenarioCategory("DerivedMC")]
            public void MethodC()
            {
            }
        }

        [Description("base")]
        class OtherBase { }
        [Description("derived")]
        class OtherDerived : OtherBase { }

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _subject = new TestableMetadataProvider();
        }

        #endregion

        [Test]
        public void Should_collect_categories_from_method_and_classes()
        {
            Assert.That(
                _subject.GetScenarioCategories(typeof(DerivedClass).GetMethod("MethodC")).ToArray(),
                Is.EqualTo(new[] { "BaseC", "DerivedC", "DerivedMC" }));
        }

        [Test]
        public void Should_collect_categories_from_method_with_base_and_classes()
        {
            Assert.That(
                _subject.GetScenarioCategories(typeof(DerivedClass).GetMethod("MethodB")).ToArray(),
                Is.EqualTo(new[] { "BaseC", "BaseMB", "DerivedC", "DerivedMB" }));
        }

        [Test]
        public void Should_collect_categories_from_base_method_and_base_class_only_if_method_is_declared_in_base()
        {
            Assert.That(
                _subject.GetScenarioCategories(typeof(DerivedClass).GetMethod("MethodA")).ToArray(),
                Is.EqualTo(new[] { "BaseC", "BaseMA" }));
        }

        [Test]
        public void Should_collect_redefined_feature_description_from_derived_class()
        {
            Assert.That(_subject.GetFeatureDescription(typeof(DerivedClass)), Is.EqualTo("DerivedDescription"));
        }

        [Test]
        public void Should_collect_redefined_description_from_derived_class()
        {
            Assert.That(_subject.GetFeatureDescription(typeof(OtherDerived)), Is.EqualTo("derived"));
        }

        [Test]
        [TestCase("given something", "GIVEN", "something")]
        [TestCase("GiVeN something", "GIVEN", "something")]
        [TestCase("when something", "WHEN", "something")]
        [TestCase("then something", "THEN", "something")]
        [TestCase("setup something", "SETUP", "something")]
        [TestCase("and something", "AND", "something")]
        [TestCase("given", "", "given")]
        [TestCase("given  ", "", "given  ")]
        [TestCase("givensomething", "", "givensomething")]
        [TestCase("xgiven", "", "xgiven")]
        public void Should_get_step_type_from_formatted_name_properly(string formattedName, string expectedType, string expectedName)
        {
            var type = _subject.GetStepTypeNameFromFormattedStepName(ref formattedName);
            Assert.That(type, Is.EqualTo(expectedType), "type");
            Assert.That(formattedName, Is.EqualTo(expectedName), "name");
        }
    }
}