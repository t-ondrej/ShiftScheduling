using ShiftScheduleLibrary.Entities;
using ShiftScheduleUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using ShiftScheduleDataAccess.Dao;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace ShiftScheduleVisualiser
{
    public partial class Form1 : Form
    {

        private IDictionary<int, double[]> DayToTimeUnits { get; }

        private string TestingDataFolder { get; set; }

        private DataAccessClient DataAccessClient { get; set; }

        private IEnumerable<Person> Persons { get; set; }

        private Requirements Requirements { get; set; }

        private ResultingSchedule ResultingSchedule { get; set; }

        private IList<int> WorkingDays { get; set; }

        public Form1()
        {
            InitializeComponent();           
            DayToTimeUnits = new Dictionary<int, double[]>();
            WorkingDays = new List<int>();

            var settings = ConfigurationManager.AppSettings;
            TestingDataFolder = settings["TestingDataFolder"];

            SeedTestDataDirectoryComboBox();
            AddEventHandlers();           
        }

        private void SeedTestDataDirectoryComboBox()
        {
            var testDataDirectories = Directory.GetDirectories(TestingDataFolder)
                .Where(directory => directory.Substring(TestingDataFolder.Length).Contains("Test"));

            testDataDirectories.ForEach(dataSet => testDataDirectoryComboBox.Items.Add(dataSet.Substring(TestingDataFolder.Length + 1)));
        }

        private void AddEventHandlers()
        {
            testDataDirectoryComboBox.SelectedIndexChanged += new EventHandler(testDataDirectoryComboBox_SelectedIndexChanged);
            dataSetComboBox.SelectedIndexChanged += new EventHandler(dataSetComboBox_SelectedIndexChanged);
            dataTypeComboBox.SelectedIndexChanged += new EventHandler(dataTypeComboBox_SelectedIndexChanged);
        }

        private void CalculateWorkers()
        {           
            var timeUnits = new double[8];
            var dayId = int.Parse(dayComboBox.SelectedItem.ToString());
            // Person by person in that day
            ResultingSchedule.DailySchedules[dayId].PersonIdToDailySchedule.ForEach(personToSchedule =>
            {
                // TimeUnit by TimeUnit
                var person = Persons.ToList().Find(p => p.Id == personToSchedule.Key);
                personToSchedule.Value.ForEach(interval =>
                {
                    var shiftWeight = person.DailyAvailabilities[dayId].ShiftWeight;
                    if (interval.Type == ShiftScheduleLibrary.Utilities.ShiftInterval.IntervalType.Pause) { shiftWeight = 0; }

                    interval.ForEach(unit => timeUnits[unit] += shiftWeight);                    
                });
            });

            DayToTimeUnits.Add(dayId, timeUnits);
        }
       

        private void showButton_Click(object sender, EventArgs e)
        {
            if (testDataDirectoryComboBox.SelectedItem == null && dataSetComboBox.SelectedItem == null
                && dataTypeComboBox.SelectedItem == null && dayComboBox.SelectedItem == null
                    && (personComboBox.Enabled && personComboBox.SelectedItem == null))
            {
                return;
            }

            var whatDataToShow = dataTypeComboBox.SelectedItem as string;

            if (whatDataToShow == "Person")
            {
                var personId = int.Parse(personComboBox.SelectedItem.ToString().Substring(7));
                var person = Persons.ToList().Find(p => p.Id == personId);

            //    Rows are not cleared for some reason thus the gridView is concatenating 
            //    new rows with the old ones :-(
            //    personScheduleGridView.Rows.Clear();
            //    personScheduleGridView.Refresh();
            //    personScheduleGridView.Invalidate();
            //    personScheduleGridView.Update();

            //    for (var i = 0; i < personScheduleGridView.RowCount; i++)
            //    {
            //       personScheduleGridView.Rows.RemoveAt(0);
            //    }

                WorkingDays.ForEach(day => 
                {
                    var rowIndex = personScheduleGridView.Rows.Add();
                    personScheduleGridView.Rows[rowIndex].HeaderCell.Value = day + ". day";

                    if (ResultingSchedule.DailySchedules[day].PersonIdToDailySchedule.ContainsKey(personId))
                    {
                        var dailyAvailability = ResultingSchedule.DailySchedules[day].PersonIdToDailySchedule[personId];
                        dailyAvailability.ForEach(interval => 
                        {
                            interval.ForEach(unit => 
                            {
                                personScheduleGridView.Rows[day].Cells[unit].Style.BackColor = System.Drawing.Color.Red;
                            });
                        });
                    }                   
                });

                Debug.WriteLine("Row count: " + personScheduleGridView.RowCount);
            }
            else
            {
                var dayId = int.Parse(dayComboBox.SelectedItem.ToString());
                CalculateWorkers();
                chart1.Series.ForEach(series => series.ChartType = SeriesChartType.Line);
                var hourToWorkers = Requirements.DaysToRequirements[dayId].HourToWorkers;
                hourToWorkers.ForEach(workerCount => chart1.Series["Requirements"].Points.AddY(workerCount));

                var timeUnits = DayToTimeUnits[dayId];
                for (var i = 0; i < 8; i++)
                {
                    if (timeUnits[i] == 0) { continue; }

                    chart1.Series["Available workers"].Points.AddY(timeUnits[i]);
                }
            }

            SwitchScene();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            SwitchScene();
            ResetForm();
        }

        private void testDataDirectoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dataSets = Directory.EnumerateDirectories(TestingDataFolder + "/" + testDataDirectoryComboBox.SelectedItem as string);

            dataSetComboBox.Items.Clear();
            dataSets.ForEach(dataSetDirectory => dataSetComboBox.Items.Add(dataSetDirectory
                .Substring(TestingDataFolder.Length + (testDataDirectoryComboBox.SelectedItem as string).Length + 2)));

            dataSetComboBox.Enabled = true;
        }

        private void dataSetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataSetComboBox.SelectedItem == null) { return; }

            DataAccessClient = new DataAccessClient(TestingDataFolder + "/" + testDataDirectoryComboBox.SelectedItem 
                + "/" + dataSetComboBox.SelectedItem as string);

            Persons = DataAccessClient.PersonDao.GetAllPersons();
            ResultingSchedule = DataAccessClient.ResultingScheduleDao.GetResultingSchedules().First();

            dataTypeComboBox.Enabled = true;
        }

        private void dataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataTypeComboBox.SelectedItem == null) { return; }

            if (dataTypeComboBox.SelectedItem as string == "Person")
            {
                // TODO: Make the person list sorted by their ID
                personComboBox.Enabled = true;
                personComboBox.Visible = true;
                personComboBox.Items.Clear();
                Persons.ForEach(person => personComboBox.Items.Add($"Person {person.Id}"));
            } else
            {
                dayComboBox.Enabled = true;               
            }

            Requirements = DataAccessClient.RequirementsDao.GetRequirements();
            Requirements.DaysToRequirements.ForEach(pair =>
            {
                WorkingDays.Add(pair.Key);
                dayComboBox.Items.Add(pair.Key);
            });
        }

        private void SwitchScene()
        {
            IList<Control> controls = new List<Control>()
            {
                formPanel, backButton
            };

            controls.ForEach(ctrl => { ctrl.Visible = !ctrl.Visible; ctrl.Enabled = !ctrl.Enabled; });

            if (dataTypeComboBox.SelectedItem as string == "Person")
            {
                personScheduleGridView.Visible = !personScheduleGridView.Visible;
                personScheduleGridView.Enabled = !personScheduleGridView.Enabled;
            } 
            else
            {
                chart1.Visible = !chart1.Visible;
                chart1.Enabled = !chart1.Enabled;
            }
        }

        private void ResetForm()
        {
            IList<ComboBox> comboBoxes = new List<ComboBox>()
            {
                dataSetComboBox, dataTypeComboBox, dayComboBox, personComboBox
            };

            comboBoxes.ForEach(box => { box.Enabled = false; box.SelectedItem = null; });

            chart1.Series["Requirements"].Points.Clear();
            chart1.Series["Available workers"].Points.Clear();
        }
    }
}
