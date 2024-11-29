

	using Unity.VisualScripting;

	public abstract class Function
	{
		private bool isable=false;

		private bool isStart=false;
		
		public bool Isable { get; set; }
		public bool IsStart{ get; set; }

		public void Start()
		{
			if (!Isable)
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
	