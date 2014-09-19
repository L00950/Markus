namespace ConsoleApplication
{
    static class UppdateraKalendern
    {
        public static void Kör()
        {
            new MarkusModel.Kalender().CreateCalendar(6);
        }

        public static void Skapafil()
        {
            new MarkusModel.Kalender().SkapaKalendrarFörObjekt(6);
        }
    }
}
