// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.5.0.0
//      Runtime Version:4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
namespace Orchard.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.5.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Content Types")]
    public partial class ContentTypesFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ContentTypes.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Content Types", "In order to add new types to my site\r\nAs an adminitrator\r\nI want to create create" +
                    " content types", GenerationTargetLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can create a new content type")]
        public virtual void ICanCreateANewContentType()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can create a new content type", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
    testRunner.Given("I have installed Orchard");
#line 8
    testRunner.When("I go to \"Admin/ContentTypes\"");
#line 9
    testRunner.Then("I should see \"<a[^>]*>.*?Create new type</a>\"");
#line 10
    testRunner.When("I go to \"Admin/ContentTypes/Create\"");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table1.AddRow(new string[] {
                        "DisplayName",
                        "Event"});
            table1.AddRow(new string[] {
                        "Name",
                        "Event"});
#line 11
        testRunner.And("I fill in", ((string)(null)), table1);
#line 15
        testRunner.And("I hit \"Create\"");
#line 16
        testRunner.And("I go to \"Admin/ContentTypes/\"");
#line 17
    testRunner.Then("I should see \"Event\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can\'t create a content type with an already existing name")]
        public virtual void ICanTCreateAContentTypeWithAnAlreadyExistingName()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can\'t create a content type with an already existing name", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
    testRunner.Given("I have installed Orchard");
#line 21
    testRunner.When("I go to \"Admin/ContentTypes/Create\"");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table2.AddRow(new string[] {
                        "DisplayName",
                        "Event"});
            table2.AddRow(new string[] {
                        "Name",
                        "Event"});
#line 22
        testRunner.And("I fill in", ((string)(null)), table2);
#line 26
        testRunner.And("I hit \"Create\"");
#line 27
        testRunner.And("I go to \"Admin/ContentTypes/\"");
#line 28
    testRunner.Then("I should see \"Event\"");
#line 29
    testRunner.When("I go to \"Admin/ContentTypes/Create\"");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table3.AddRow(new string[] {
                        "DisplayName",
                        "Event"});
            table3.AddRow(new string[] {
                        "Name",
                        "Event-2"});
#line 30
        testRunner.And("I fill in", ((string)(null)), table3);
#line 34
        testRunner.And("I hit \"Create\"");
#line 35
    testRunner.Then("I should see \"<h1[^>]*>.*?New Content Type.*?</h1>\"");
#line 36
        testRunner.And("I should see \"validation-summary-errors\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can\'t create a content type with an already existing technical name")]
        public virtual void ICanTCreateAContentTypeWithAnAlreadyExistingTechnicalName()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can\'t create a content type with an already existing technical name", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 39
    testRunner.Given("I have installed Orchard");
#line 40
    testRunner.When("I go to \"Admin/ContentTypes/Create\"");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table4.AddRow(new string[] {
                        "DisplayName",
                        "Dinner"});
            table4.AddRow(new string[] {
                        "Name",
                        "Dinner"});
#line 41
        testRunner.And("I fill in", ((string)(null)), table4);
#line 45
        testRunner.And("I hit \"Create\"");
#line 46
        testRunner.And("I go to \"Admin/ContentTypes/\"");
#line 47
    testRunner.Then("I should see \"Dinner\"");
#line 48
    testRunner.When("I go to \"Admin/ContentTypes/Create\"");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table5.AddRow(new string[] {
                        "DisplayName",
                        "Dinner2"});
            table5.AddRow(new string[] {
                        "Name",
                        "Dinner"});
#line 49
        testRunner.And("I fill in", ((string)(null)), table5);
#line 53
        testRunner.And("I hit \"Create\"");
#line 54
    testRunner.Then("I should see \"<h1[^>]*>.*?New Content Type.*?</h1>\"");
#line 55
        testRunner.And("I should see \"validation-summary-errors\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can\'t rename a content type with an already existing name")]
        public virtual void ICanTRenameAContentTypeWithAnAlreadyExistingName()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can\'t rename a content type with an already existing name", ((string[])(null)));
#line 57
this.ScenarioSetup(scenarioInfo);
#line 58
    testRunner.Given("I have installed Orchard");
#line 59
    testRunner.When("I go to \"Admin/ContentTypes/Create\"");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table6.AddRow(new string[] {
                        "DisplayName",
                        "Dinner"});
            table6.AddRow(new string[] {
                        "Name",
                        "Dinner"});
#line 60
        testRunner.And("I fill in", ((string)(null)), table6);
#line 64
        testRunner.And("I hit \"Create\"");
#line 65
        testRunner.And("I go to \"Admin/ContentTypes/\"");
#line 66
    testRunner.Then("I should see \"Dinner\"");
#line 67
    testRunner.When("I go to \"Admin/ContentTypes/Create\"");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table7.AddRow(new string[] {
                        "DisplayName",
                        "Event"});
            table7.AddRow(new string[] {
                        "Name",
                        "Event"});
#line 68
        testRunner.And("I fill in", ((string)(null)), table7);
#line 72
        testRunner.And("I hit \"Create\"");
#line 73
        testRunner.And("I go to \"Admin/ContentTypes/\"");
#line 74
    testRunner.Then("I should see \"Event\"");
#line 75
    testRunner.When("I go to \"Admin/ContentTypes/Edit/Dinner\"");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table8.AddRow(new string[] {
                        "DisplayName",
                        "Event"});
#line 76
        testRunner.And("I fill in", ((string)(null)), table8);
#line 79
        testRunner.And("I hit \"Save\"");
#line 80
    testRunner.Then("I should see \"validation-summary-errors\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
    }
}
#endregion
