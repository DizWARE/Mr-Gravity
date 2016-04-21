namespace MrGravity.LevelEditor.IOperationClasses
{
    public interface IOperation
    {
        /*
         * Redo
         * 
         * Interface method implemented by IOperation classes.
         */
        void Redo();

        /*
         * Undo
         * 
         * Interface method implemented by IOperation classes.
         */
        void Undo();
    }
}
