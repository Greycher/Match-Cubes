public struct BoardCoordinate {
    public uint rowIndex;
    public uint columnIndex;

    public BoardCoordinate(uint rowIndex, uint columnIndex) {
        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
    }

    public BoardCoordinate Up() {
        return new BoardCoordinate(rowIndex + 1, columnIndex);
    }
    
    public BoardCoordinate Right() {
        return new BoardCoordinate(rowIndex, columnIndex + 1);
    }
    
    public BoardCoordinate Down() {
        return new BoardCoordinate(rowIndex - 1, columnIndex);
    }
    
    public BoardCoordinate Left() {
        return new BoardCoordinate(rowIndex, columnIndex - 1);
    }
}
