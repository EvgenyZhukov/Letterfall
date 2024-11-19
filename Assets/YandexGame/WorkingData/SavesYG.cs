namespace YG
{
    /*
    [System.Serializable]
    public class LevelData
    {
        public float[][] positions;
        public char[] letters;
        public int[] spawnerNumber;
    }
    */
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Ваши сохранения

        //public int score;
        public int maxscore;
        //public int maxwords;

        public int bonus_01;
        public int bonus_02;
        public int bonus_03;

        //public bool secondChance;
        //public bool gameStarted;

        //public LevelData levelData;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {

        }
    }
}
