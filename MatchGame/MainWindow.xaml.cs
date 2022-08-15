using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading; //Библиотека таймера
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer(); //Создать новый таймер
        int tenthsOfSecondsElapsed; //Отслеживать время
        int matchesFound; //Количество найденных совпадений
        public MainWindow()
        {
            timer.Interval = TimeSpan.FromSeconds(.1);//Частота срабатывания
            timer.Tick += Timer_Tick; //Какой метод вызываеся при срабатывании

            InitializeComponent();
            SetUpGame(); //Добавляю метод рисования эмодзи
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed--;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0c");//Записываю время в таком-то формате и переобразовываю в строку
            if ((matchesFound == 8 | (tenthsOfSecondsElapsed == 0))) //Если найдены 8 пар то
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Играть снова?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>() //Создает список из восьми пар эмодзи
            {
                "😊", "😊",
                "🐵", "🐵",
                "🦊", "🦊",
                "🦁", "🦁",
                "🐽", "🐽",
                "❤️", "❤️",
                "👍", "👍",
                "👌", "👌",
            };
            Random random = new Random();                   //Создает новый генератор случайных чисел
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())      //Находит каждый элемент TextBlock в сетке и повторяет следующие команды для каждого элемента
            {
                if (textBlock.Name != "timeTextBlock") //Если не блок времени
                {
                    textBlock.Visibility = Visibility.Visible; //Делает видимым объект
                    int index = random.Next(animalEmoji.Count); //Выбирает случайное число от 0 до количества эмодзи в списке и назначает ему имя «index»
                    string nextEmoji = animalEmoji[index];      //Использует случайное число с именем «index» для получения случайного эмодзи из списка
                    textBlock.Text = nextEmoji;                 //Обновляет TextBlock случайным эмодзи из спис
                    animalEmoji.RemoveAt(index);              //Удаляет случайный эмодзи из списка
                }
            }

            timer.Start(); //Начинает таймер
            tenthsOfSecondsElapsed = 50; //Сбрасывает время
            matchesFound = 0; //Сбрасывает Кол-во пар
        }

        TextBlock lastTextBlockClicked; // Добавляю объект класса TextBlock
        bool findingMatch = false; //Добавляю объект класса bool в значении 0
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock; //Вроде как ссылка на класс откуда взяли, чтобы не писать обработчик щелчков для 16 TextBox
            if (findingMatch == false) // Если первый щелчёк то
            {
                textBlock.Visibility = Visibility.Hidden; //Делаю невидимым объект
                lastTextBlockClicked = textBlock; //Записываю его в левый
                findingMatch = true; // Меняю на второй щелчёк
            }
            else if (textBlock.Text == lastTextBlockClicked.Text) //Если текс записанный в выбранном текст боксе и левом текст боксе динаковый то
            {
                matchesFound++; //Считает кол-во пар
                textBlock.Visibility = Visibility.Hidden; //Делаю невидимым объект
                findingMatch = false; //Меняю на 1 шелчок
                tenthsOfSecondsElapsed += 10;//Прибавляю время 1 с за пару
            }
            else //Иначе.... Т.е. Если текст разный на 2 щелчок то
            {
                lastTextBlockClicked.Visibility = Visibility.Visible; //Делаю левый видимым
                findingMatch = false; //Меняю на 1 щелчок
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)//Метод кликанья на таймер для начала новой игры
        {
            if ((matchesFound == 8 | (tenthsOfSecondsElapsed == 0)))  //Если нашёл 8 пар
            {
                SetUpGame();//Начинает игру по новой
            }
        }
    }
}
