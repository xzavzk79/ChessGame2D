using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : BasePieces
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        GetComponent<Image>().sprite = Resources.Load<Sprite>("Knight");
    }

    private void CreateCellPath(int flipper)
    {
        
        //позиция фигуры
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Влево
        MatchesState(currentX - 2, currentY + (1 * flipper));

        //Лево Верх
        MatchesState(currentX-1, currentY + (2 * flipper));

        //Право Верх
        MatchesState(currentX + 1, currentY + (2 * flipper));

        //Вправо
        MatchesState(currentX + 2, currentY + (1 * flipper));
    }
    protected override void CheckPathing()
    {
        //Рисуем верхнуюю половину ходов
        CreateCellPath(1);
        //Рисуем нижнюю половину ходов
        CreateCellPath(-1);
    }

    private void MatchesState(int targetX, int targetY)
    {
        CellState cellState = CellState.None;
        cellState = mCurrentCell.mBoard.ValidateCell(targetX, targetY, this);

        if (cellState != CellState.Friendly && cellState != CellState.OutOfBounds)
        {
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
        }
    }
}
