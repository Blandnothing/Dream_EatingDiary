

	using Unity.VisualScripting;

	public abstract class Function
	{
		public bool isable=false;

		public bool isStart=false;

		public void Start()
		{
			if (!isable)
			{
				return;
			}
			isStart = true;
			StartFunction();
		}

		
		public void Stop()
        {
            isStart = false;
            StopFunction();
        }
		public abstract void StartFunction();
		public abstract void StopFunction();

		
	}
	