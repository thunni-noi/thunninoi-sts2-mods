using Godot;

/// <summary>
/// ScrollingLabel — attach to a Label node.
///
/// When the label's text is wider than its parent container the text will
/// smoothly scroll left until the end is visible, pause, scroll back, and
/// repeat (ping-pong / marquee behaviour).
///
/// SETUP
/// ─────
///  1. Add a Control node (e.g. Panel, SubViewport container) as the clip box.
///     Enable  Clip Contents = true  in its Inspector.
///  2. Add a Label as a DIRECT child of that Control.
///  3. Set the Label's anchor to Top-Left, position (0,0), no size flags stretch.
///  4. Attach this script to the Label.
///  5. Set  Autowrap Mode = Off  on the Label (the script also enforces this).
///
/// COMMON GOTCHA (Godot 4 + C#)
/// ─────────────────────────────
///  Exported properties won't appear in the Inspector until you build the project:
///  Top menu → Build → Build Project  (or Ctrl + Shift + B).
/// </summary>
public partial class ScrollingLabel : Label
{
	// ── Inspector exports ────────────────────────────────────────────────

	[Export] public float ScrollSpeed   = 60f;   // pixels per second
	[Export] public float PauseDuration = 1.5f;  // seconds to wait at each end
	[Export] public float EdgePadding   = 16f;   // extra gap at the right end

	// ── State machine ────────────────────────────────────────────────────

	private enum State { Idle, PauseAtStart, ScrollLeft, PauseAtEnd, ScrollRight }

	private State _state      = State.Idle;
	private float _offsetX    = 0f;
	private float _maxOffset  = 0f;
	private float _pauseTimer = 0f;

	// Track last known values so we only recalculate on actual changes.
	private string _lastText           = "";
	private float  _lastContainerWidth = 0f;

	// ── Godot lifecycle ──────────────────────────────────────────────────

	public override void _Ready()
	{
		GD.Print("Ready!");
		AutowrapMode = TextServer.AutowrapMode.Off;
		ClipText     = false;

		// Wait one extra frame so Godot has finished its first layout pass.
		CallDeferred(MethodName.FirstEvaluate);
	}

	private void FirstEvaluate() => CheckForChanges();

	public override void _Process(double delta)
	{
		CheckForChanges();

		if (_state == State.Idle)
			return;

		float dt = (float)delta;

		switch (_state)
		{
			case State.PauseAtStart:
				_pauseTimer -= dt;
				if (_pauseTimer <= 0f)
					_state = State.ScrollLeft;
				break;

			case State.ScrollLeft:
				_offsetX -= ScrollSpeed * dt;
				if (_offsetX <= -_maxOffset)
				{
					_offsetX    = -_maxOffset;
					_pauseTimer = PauseDuration;
					_state      = State.PauseAtEnd;
				}
				break;

			case State.PauseAtEnd:
				_pauseTimer -= dt;
				if (_pauseTimer <= 0f)
					_state = State.ScrollRight;
				break;

			case State.ScrollRight:
				_offsetX += ScrollSpeed * dt;
				if (_offsetX >= 0f)
				{
					_offsetX    = 0f;
					_pauseTimer = PauseDuration;
					_state      = State.PauseAtStart;
				}
				break;
		}

		Position = new Vector2(_offsetX, Position.Y);
	}

	// ── Helpers ──────────────────────────────────────────────────────────

	/// <summary>
	/// Recalculate only when text or container width actually changed,
	/// so we don't reset the scroll position every frame.
	/// </summary>
	private void CheckForChanges()
	{
		float containerWidth = GetParentControl()?.Size.X ?? 0f;

		if (Text == _lastText && Mathf.IsEqualApprox(containerWidth, _lastContainerWidth))
			return;

		_lastText           = Text;
		_lastContainerWidth = containerWidth;

		// KEY FIX: GetMinimumSize().X returns the true pixel width of the
		// rendered text, regardless of how wide the Label control itself is.
		float textWidth = GetMinimumSize().X;
		float overflow  = textWidth - containerWidth;

		if (overflow <= 0f)
		{
			// Text fits — reset.
			_state   = State.Idle;
			_offsetX = 0f;
			Position = new Vector2(0f, Position.Y);
		}
		else
		{
			// Text overflows — start (or restart) scrolling.
			_maxOffset = overflow + EdgePadding;

			// Only restart from the beginning if we were previously idle.
			if (_state == State.Idle)
			{
				_offsetX    = 0f;
				_pauseTimer = PauseDuration;
				_state      = State.PauseAtStart;
			}
		}
	}
}
