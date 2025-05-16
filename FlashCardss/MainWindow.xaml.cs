using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Data;

namespace FlashCardss
{
	
	public partial class MainWindow : Window
	{
		private int amountOf_Correct;
		private int amountOf_Incorrect;
		private int currentQuestionIndex;


		private string pathJson;


		private bool question_Changed;

		private List<QAEntry> qaList;

		private string AssemblyLine
		{
			get
			{
				try
				{
					return System.Reflection.Assembly.GetExecutingAssembly().Location;
				
				}
				catch { }

				return "Unkown";
			}
		}

		public MainWindow()
		{
			InitializeComponent();
			amountOf_Correct = 0;
			amountOf_Incorrect = 0;

			pathJson = System.IO.Path.Combine(
					System.IO.Path.GetDirectoryName(AssemblyLine)!,
					"Data", "Packs","csharpqa.json");

			qaList  = JsonConvert.DeserializeObject<List<QAEntry>>(File.ReadAllText(pathJson))!;

			question_Changed = true;
			NextQuestion();	


		}



		public class QAEntry
		{
			public string question { get; set; }
			public string answer { get; set; }
		}

		public class QAResult
		{
			public int Index { get; set; }
			public int Num { get; set; }
			public bool IsCorrect { get; set; }
		}

		private void wrongbutton_Click(object sender, RoutedEventArgs e)
		{
			if (question_Changed) { amountOf_Incorrect++; question_Changed = false; }
		}

		private void correctbutton_Click(object sender, RoutedEventArgs e)
		{
			if (question_Changed) { amountOf_Correct++; question_Changed = false; qaList.RemoveAt(currentQuestionIndex); }
		}
		private void homebutton_Click(object sender, RoutedEventArgs e)
		{
			
		}
		private void enterbutton_Click(object sender, RoutedEventArgs e) 
		{
			System.Diagnostics.Debug.WriteLine("Amount of correct you have: " + amountOf_Correct + " And amount of Incorrect you have: " +  amountOf_Incorrect);
			NextQuestion();
		}

		private void revealbutton_Click(object sender, RoutedEventArgs e)
		{
			answerBlock.Visibility = 
				(answerBlock.Visibility == Visibility.Visible)
				? Visibility.Hidden
				: Visibility.Visible;
		}

		

		private void NextQuestion()
		{
			if(qaList != null && qaList.Count != 0)
			{
				answerBlock.Visibility = Visibility.Hidden;



				Random rnd = new Random();
				int num = rnd.Next(0, qaList.Count());



				currentQuestionIndex = num;
				questionBlock.Text = qaList[num].question;
				answerBlock.Text = qaList[num].answer;

				question_Changed = true;
			} else
			{
				System.Diagnostics.Debug.WriteLine("You completed all questions!");
				questionBlock.Text = "All done!";
				answerBlock.Text = "";
				return;
			}


		}
		
	}
}