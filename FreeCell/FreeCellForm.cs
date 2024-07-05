
using FreeCellLibrary;
using System.Resources;

namespace FreeCell;

public partial class FreeCellForm : Form
{
    int cardWidth = 125;
    int cardHeight = 174;

    int freecellX = 77;
    int freecellY = 37;
    int freecellDistance = 164;
    int homeX = 782;
    int homeY = 37;
    int homeDistance = 164;
    int columnX = 77;
    int columnY = 343;
    int columnHorizontalDistance = 171;
    int cardVerticalDistance = 46;
    bool spaceModifier = false;

    ResourceManager _resourceManager = Properties.Resources.ResourceManager;
    public GameBoard gameBoard { get; set; }
    public FreeCellForm()
    {
        InitializeComponent();
        gameBoard = new GameBoard();

        int xPos = columnX;
        for (int i = 0; i < 8; i++)
        {
            Card card = gameBoard.Columns[i].PeekBottom();
            DrawColumn(i, xPos, columnY, card);
            xPos += columnHorizontalDistance;
        }


        gameBoard.PostAction();
    }

    public void DrawBoard()
    {
        //Columns
        int xPos = columnX;
        for (int i = 0; i < 8; i++)
        {
            Card card = gameBoard.Columns[i].PeekBottom();
            DrawColumn(i, xPos, columnY, card);
            xPos += columnHorizontalDistance;
        }


        int freecellXPos = freecellX;
        for (int i = 0; i < 4; i++)
        {
            if (gameBoard.Freecells[i].Top is not null)
            {
                Card card = gameBoard.Freecells[i].Top;
                DrawFreecell(freecellXPos, card);
                gameBoard.numOfEmptyFreeCells++;
            }
            freecellXPos += freecellDistance;
        }

        int homeXPos = homeX;
        for (int i = 0; i < 4; i++)
        {
            if (gameBoard.Homes[i].Top is not null)
            {
                Card card = gameBoard.Homes[i].Top;
                DrawHome(homeXPos, card);
            }
            homeXPos += homeDistance;
        }
    }

    private void DrawHome(int xPos, Card? card)
    {
        Bitmap image = (Bitmap)_resourceManager.GetObject(card.Name);
        PictureBox testBox = new PictureBox();
        testBox.Image = image;
        testBox.Width = cardWidth;
        testBox.Height = cardHeight;
        testBox.Name = card.Name;
        testBox.Location = new Point(xPos, homeY);
        this.Controls.Add(testBox);
        testBox.BringToFront();
    }

    private void DrawFreecell(int xPos, Card? card)
    {
        Bitmap image = (Bitmap)_resourceManager.GetObject(card.Name);
        PictureBox testBox = new PictureBox();
        testBox.Image = image;
        testBox.Width = cardWidth;
        testBox.Height = cardHeight;
        testBox.Name = card.Name;
        testBox.Location = new Point(xPos, freecellY);
        this.Controls.Add(testBox);
        testBox.BringToFront();
    }

    public void DrawColumn(int columnNumber, int xPos, int yPos, Card card)
    {
        Bitmap image = (Bitmap)_resourceManager.GetObject(card.Name);
        PictureBox testBox = new PictureBox();
        testBox.Image = image;
        testBox.Width = cardWidth;
        testBox.Height = cardHeight;
        testBox.Name = card.Name;
        testBox.Location = new Point(xPos, yPos);
        this.Controls.Add(testBox);
        testBox.BringToFront();

        if (card.Up is not null)
        {
            card = card.Up;
            DrawColumn(columnNumber, xPos, yPos + cardVerticalDistance, card);
        }
    }

    private void FreeCellForm_KeyPress(object sender, KeyPressEventArgs e)
    {
        switch (e.KeyChar)
        {
            case 'q':
                gameBoard.PrimaryMove('q');
                DrawBoard();
                break;
            case 'w':
                gameBoard.PrimaryMove('w');
                DrawBoard();
                break;
            case 'e':
                gameBoard.PrimaryMove('e');
                DrawBoard();
                break;
            case 'r':
                gameBoard.PrimaryMove('r');
                DrawBoard();
                break;

            case 'u':
                gameBoard.PrimaryMove('u');
                DrawBoard();
                break;
            case 'i':
                gameBoard.PrimaryMove('i');
                DrawBoard();
                break;
            case 'o':
                gameBoard.PrimaryMove('o');
                DrawBoard();
                break;
            case 'p':
                gameBoard.PrimaryMove('p');
                DrawBoard();

                break;
            case 'a':
                gameBoard.PrimaryMove('a');
                DrawBoard();
                break;
            case 's':
                gameBoard.PrimaryMove('s');
                DrawBoard();
                break;
            case 'd':
                gameBoard.PrimaryMove('d');
                DrawBoard();
                break;
            case 'f':
                gameBoard.PrimaryMove('f');
                DrawBoard();
                break;
            case 'j':
                gameBoard.PrimaryMove('j');
                DrawBoard();
                break;
            case 'k':
                gameBoard.PrimaryMove('k');
                DrawBoard();
                break;
            case 'l':
                gameBoard.PrimaryMove('l');
                DrawBoard();
                break;
            case ';':
                gameBoard.PrimaryMove(';');
                DrawBoard();

                break;
            case 'g':
                break;
            case 'h':
                break;
            case 'z':
                break;
            case 'x':
                break;

        } 

    }

    private void FreeCellForm_KeyDown(object sender, KeyEventArgs e)
    {
        //if(e.KeyCode == "space")

    }


    private void FreeCellForm_KeyUp(object sender, KeyEventArgs e)
    {

    }
}
