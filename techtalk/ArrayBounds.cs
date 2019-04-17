
namespace techtalk
{
    class ArrayBounds
    {

        public void arraybounds()
        {
            var arr = new int[100];

            //Array bounds probably not elided
            for (int i = 0; i < 100; i++)
            {
                var x = arr[i];
            }

            //Array bounds probably not elided
            int arrLen = arr.Length;
            for (int i = 0; i < arrLen; i++)
            {
                var x = arr[i];
            }

            //Array bounds probably not elided            
            for (int i = arrLen-1; i >= 0; i--)
            {
                var x = arr[i];
            }

            //Array bounds elided!
            for (int i = 0; i < arr.Length; i++)
            {
                var x = arr[i];
            }
            
            //No way to get this elided
            for (int i = 0; i < 50; i++)
            {
                var x = arr[i];
            }

            //Except to use unsafe!
            unsafe
            {
                fixed (int* a = arr)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        var x = a[i];
                    }
                }
            }

        }

        
    }
}
