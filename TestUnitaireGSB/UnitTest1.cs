using Microsoft.VisualStudio.TestTools.UnitTesting;
using GSB;

namespace TestUnitaireGSB 
{
    [TestClass]
    public class UnitTest1 
    {

        GestionDate x = new GestionDate();

        [TestMethod]
        public void TestMethod1()
        {

            Assert.AreEqual("202103", x.getDateDuJour(), "echec erreur");
            Assert.AreEqual("202103", x.getDateMoisPrecedent(), "echec erreur");
            


        }

       
    }

}
