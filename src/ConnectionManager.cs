using Godot;
using System;

public partial class ConnectionManager : Node
{
	[Export] private Button _hostButton;
	[Export] private Button _joinButton;
	[Export] private Button _pingButton;
	[Export] private Button _ipListButton;
	
	[Export] private TextEdit _hostAddress;
	[Export] private Label _output;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hostButton.ButtonDown += OnHostButtonPressed;
		_joinButton.ButtonDown += OnJoinButtonPressed;
		_ipListButton.ButtonDown += OnIPListButtonPressed;
		_pingButton.ButtonDown += OnPingButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		
	}

	private void OnHostButtonPressed()
	{
		ENetMultiplayerPeer peer;
		if (Multiplayer.MultiplayerPeer is ENetMultiplayerPeer multiplayerPeer)
		{
			peer = multiplayerPeer;
			peer.Close();
			PrintLn("Closed existing connection");
		}
		else
		{
			peer = new ENetMultiplayerPeer();
		}
		var err = peer.CreateServer(6767);
		if (err != Error.Ok)
		{
			PrintLn("Error starting server: " + err);
			return;
		}
		Multiplayer.MultiplayerPeer = peer;
		PrintLn("Server started");
		
	}

	private void OnJoinButtonPressed()
	{
		ENetMultiplayerPeer peer;
		if (Multiplayer.MultiplayerPeer is ENetMultiplayerPeer multiplayerPeer)
		{
			peer = multiplayerPeer;
			peer.Close();
			PrintLn("Closed existing connection");
		}
		else
		{
			peer = new ENetMultiplayerPeer();
		}
		var err = peer.CreateClient(_hostAddress.Text, 6767);
		if (err != Error.Ok)
		{
			PrintLn("Error starting client: " + err);
			return;
		}
		Multiplayer.MultiplayerPeer = peer;
		PrintLn("Client started");
	}

	private void OnPingButtonPressed()
	{
		PrintLn("Pinging...");
		Rpc(nameof(Ping));
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 0)]
	private void Ping()
	{
		PrintLn("Ping");
		Rpc(nameof(Pong));
	} 
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void Pong()
	{
		PrintLn("Pong");
	}
	
	private void PrintLn(string s)
	{
		_output.Text += Time.GetTimeStringFromSystem() + ": " + s + "\n";
	}
	
	private void OnIPListButtonPressed()
	{
		PrintLn("List of local ips:");
        foreach(var ip in IP.GetLocalAddresses())
        {
        	PrintLn("'"+ip+"'");
        }
	}
}
