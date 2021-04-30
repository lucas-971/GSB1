using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using System.Timers;

namespace GSB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
        }
        private String unProvider = "localhost";
        private String uneDataBase = "gsb";
        private String unUid = "root";
        private String unMdp = "";
        private String dateTest;
        private int dateJour;
        ConnexionSql maConnexion;
        // private System.Timers.Timer myTimer=null;

        GestionDate GD = new GestionDate();



        private MySqlCommand maCo;


        
        private void affiche()
        {
            maCo = maConnexion.reqExec("Select * from fichefrais Where mois ='"+ GD.getDateMoisPrecedent()+"'");

            MySqlDataReader reader = maCo.ExecuteReader();


            DataTable dt = new DataTable();
            for (int i = 0; i <= reader.FieldCount - 1; i++)
            {

                dt.Columns.Add(reader.GetName(i));

            }

            while (reader.Read())
            {

                DataRow dr = dt.NewRow();

                for (int i = 0; i <= reader.FieldCount - 1; i++)
                {
                    dr[i] = reader.GetValue(i);


                }

                dt.Rows.Add(dr);

            }

            //dt.Load(reader); 


            dgv1.DataSource = dt;

            reader.Close();
            maConnexion.closeConnection();

        }

       

        private void Form1_Load_2(object sender, EventArgs e)
        {
            maConnexion = ConnexionSql.getInstance(unProvider, uneDataBase, unUid, unMdp);
            maConnexion.openConnection();
            affiche();
            maConnexion.closeConnection();
            lbDateJour.Text = GD.getDateDuJour();

        }

        private void ChangeEtat_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(GD.getJour())<=10){
                maCo = maConnexion.reqExec(" Update fichefrais set idEtat = 'CL' where idVisiteur = '" + idVisiteur.Text + "' and idEtat ='CR'");
                DataTable dt = new DataTable();
                dt.Load(maCo.ExecuteReader());
                affiche();
            }
            else if (Convert.ToInt32(GD.getJour()) <= 31 | Convert.ToInt32(GD.getJour()) >= 20)
            {
                maCo = maConnexion.reqExec(" Update fichefrais set idEtat = 'RB' where idVisiteur = '" + idVisiteur.Text + "' and idEtat ='VA'");
                DataTable dt = new DataTable();
                dt.Load(maCo.ExecuteReader());
                affiche();
            }
        }

        private void InitializeTimer()
        {
            timer1.Interval = 10000;
            timer1.Tick += new EventHandler(timer1_Tick);

            // Enable timer.  
            timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            try
            {

             
                dateJour = Convert.ToInt32(GD.getJour());


                if (dateJour >= 1 && dateJour <= 10)
                {
                    maConnexion.openConnection();


                    maCo = maConnexion.reqExec("update fichefrais set idEtat ='CL' where idEtat ='CR' and mois = '" + GD.getDateMoisPrecedent() + "'");
                    maCo.ExecuteNonQuery();


                    dateTest = GD.getDateMoisPrecedent();

                    maCo = maConnexion.reqExec("Select * from fichefrais where mois='" + dateTest + "'");
                    DataTable dt = new DataTable();
                    dt.Load(maCo.ExecuteReader());

                    dgv1.DataSource = dt;

                    maConnexion.closeConnection();
                }
                else if (dateJour >= 20 && dateJour <= 31)
                {
                    maConnexion.openConnection();

                    maCo = maConnexion.reqExec("update fichefrais set idEtat ='VA' where idEtat ='RB' and mois = '" + GD.getDateMoisPrecedent() + "'");
                    maCo.ExecuteNonQuery();

                   
                    dateTest = GD.getDateMoisPrecedent();

                    maCo = maConnexion.reqExec("Select * from fichefrais where mois='" + dateTest + "'");
                    DataTable dt = new DataTable();
                    dt.Load(maCo.ExecuteReader());

                    dgv1.DataSource = dt;

                    maConnexion.closeConnection();
                }

            }
            catch (Exception emp)
            {
                MessageBox.Show(emp.Message);
            }
        }
    }
}
