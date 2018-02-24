namespace Jammit.Model
{
  public class Looper : ISynchronizable
  {
    const double MinimumLoop = 0.25;
    private double _loopStart = 0.0;
    private double _loopEnd = 0.0;
    public bool Enabled { get; set; } = false;

    public double LoopStart
    {
      get { return _loopStart; }
      set
      {
        _loopStart = value;
        if (_loopEnd < _loopStart + MinimumLoop)
        {
          _loopEnd = _loopStart + MinimumLoop;
        }
      }
    }

    public double LoopEnd
    {
      get { return _loopEnd; }
      set
      {
        _loopEnd = value;
        if (_loopEnd < _loopStart + MinimumLoop)
        {
          _loopEnd = _loopStart + MinimumLoop;
        }
      }
    }

    public long SamplePosition
    {
      set
      {
        if (Enabled && value/44100.0 >= LoopEnd)
        {
          OnLoop?.Invoke(LoopStart);
        }
      }
    }

    public long Samples { get; set; }

    public delegate void LoopEvent(double loopTo);
    public event LoopEvent OnLoop;

  }
}