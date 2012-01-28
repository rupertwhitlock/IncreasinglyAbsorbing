// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.4.0.0
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
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Lists")]
    public partial class ListsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Lists.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Lists", "In order to add new lists to my site\r\nAs an administrator\r\nI want to create lists" +
                    "", GenerationTargetLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("I can create a new list")]
        public virtual void ICanCreateANewList()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can create a new list", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
    testRunner.Given("I have installed Orchard");
#line 8
    testRunner.When("I go to \"Admin/Contents/Create/List\"");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table1.AddRow(new string[] {
                        "Routable.Title",
                        "MyList"});
#line 9
        testRunner.And("I fill in", ((string)(null)), table1);
#line 12
        testRunner.And("I hit \"Save\"");
#line 13
        testRunner.And("I go to \"Admin/Contents/List/List\"");
#line 14
    testRunner.Then("I should see \"MyList\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can add content items to a list")]
        public virtual void ICanAddContentItemsToAList()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can add content items to a list", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
    testRunner.Given("I have installed Orchard");
#line 18
        testRunner.And("I have a containable content type \"MyType\"");
#line 19
    testRunner.When("I go to \"Admin/Contents/Create/List\"");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table2.AddRow(new string[] {
                        "Routable.Title",
                        "MyList"});
#line 20
        testRunner.And("I fill in", ((string)(null)), table2);
#line 23
        testRunner.And("I hit \"Save\"");
#line 24
        testRunner.And("I go to \"Admin/Contents/List/List\"");
#line 25
    testRunner.Then("I should see \"MyList\"");
#line 26
    testRunner.When("I follow \"Contained Items\"");
#line 27
    testRunner.Then("I should see \"The \'MyList\' List has no content items.\"");
#line 28
    testRunner.When("I follow \"Create New Content\" where href has \"ReturnUrl\"");
#line 29
    testRunner.Then("I should see \"MyType\"");
#line 30
    testRunner.When("I follow \"MyType\" where href has \"ReturnUrl\"");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table3.AddRow(new string[] {
                        "Routable.Title",
                        "MyContentItem"});
#line 31
        testRunner.And("I fill in", ((string)(null)), table3);
#line 34
        testRunner.And("I hit \"Save\"");
#line 35
        testRunner.And("I am redirected");
#line 36
    testRunner.Then("I should see \"Manage MyList\"");
#line 37
        testRunner.And("I should see \"MyContentItem\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
    }
}
#endregion
