using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }

    public enum DrivingAspect
    {
        None,
        Width,
        Height
    }

    public FitType fitType;

    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public float maxWidth;
    public bool fitY;
    public float maxHeight;

    public DrivingAspect drivingAspect;
    public float aspectRatio;

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns || fitType == FitType.Uniform)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows || fitType == FitType.Uniform)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = (fitX && columns != 0) ? cellWidth : cellSize.x;
        cellSize.y = (fitY && rows != 0) ? cellHeight : cellSize.y;


        if (maxWidth < cellSize.x)
        {
            cellSize.x = maxWidth;
        }
        if (maxHeight < cellSize.y)
        {
            cellSize.y = maxHeight;
        }


        if (drivingAspect == DrivingAspect.Width)
        {
            cellSize.y = cellSize.x * aspectRatio;
        }
        else if (drivingAspect == DrivingAspect.Height)
        {
            cellSize.x = cellSize.y * aspectRatio;
        }


        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
}