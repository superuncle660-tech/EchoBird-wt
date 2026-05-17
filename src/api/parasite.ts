// Parasite APIs — Mother Agent's "parasite" mode wraps installed CLI agents
// (Hermes / Claude Code / OpenClaw) as a drop-in alternative to EchoBird's
// own agent_loop. The frontend toggles into parasite mode per agent and
// streams responses back via `parasite_event`.
import { invoke } from '@tauri-apps/api/core';
import { listen, type UnlistenFn } from '@tauri-apps/api/event';
import type { ParasiteSendRequest, ParasiteEvent } from './types';

export async function parasiteListInstalled(): Promise<string[]> {
  return invoke('parasite_list_installed');
}

export async function parasiteSendMessage(request: ParasiteSendRequest): Promise<void> {
  return invoke('parasite_send_message', { request });
}

export async function parasiteAbort(agentId: string): Promise<boolean> {
  return invoke('parasite_abort', { agentId });
}

export async function parasiteReset(agentId: string): Promise<void> {
  return invoke('parasite_reset', { agentId });
}

export function listenParasiteEvents(handler: (event: ParasiteEvent) => void): Promise<UnlistenFn> {
  return listen<ParasiteEvent>('parasite_event', (e) => handler(e.payload));
}
