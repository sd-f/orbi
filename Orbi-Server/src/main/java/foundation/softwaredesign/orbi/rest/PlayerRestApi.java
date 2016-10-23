package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.game.character.CharacterMessage;
import foundation.softwaredesign.orbi.model.game.character.CharacterMessages;
import foundation.softwaredesign.orbi.model.game.character.Inventory;
import foundation.softwaredesign.orbi.model.game.character.Player;
import foundation.softwaredesign.orbi.model.game.transform.Transform;
import foundation.softwaredesign.orbi.model.game.world.World;
import foundation.softwaredesign.orbi.service.game.character.CharacterMessageService;
import foundation.softwaredesign.orbi.service.game.world.ElevationService;
import foundation.softwaredesign.orbi.service.game.character.PlayerService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.transaction.Transactional;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;

import static java.util.Objects.isNull;
import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/player")
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
@RequestScoped
public class PlayerRestApi {

    @Inject
    ElevationService elevationService;
    @Inject
    PlayerService playerService;
    @Inject
    CharacterMessageService characterMessageService;

    private void checkPlayerParameter(Player player) {
        if (isNull(player)) {
            throw new BadRequestException();
        }
        if (isNull(player.getCharacter())) {
            throw new BadRequestException();
        }
        if (isNull(player.getCharacter().getTransform())) {
            throw new BadRequestException();
        }
        if (isNull(player.getCharacter().getTransform().getGeoPosition())) {
            throw new BadRequestException();
        }
        if (isNull(player.getCharacter().getTransform().getRotation())) {
            throw new BadRequestException();
        }
    }

    private void checkMessageParameter(CharacterMessage message) {
        if (isNull(message)) {
            throw new BadRequestException();
        }
        if (isNull(message.getToCharacter())) {
            throw new BadRequestException();
        }
        if (isNull(message.getMessage())) {
            throw new BadRequestException();
        }
        if (message.getMessage().isEmpty()) {
            throw new BadRequestException();
        }
    }

    @POST
    @Path("/craft")
    @Transactional
    public World craft(@NotNull Player player) {
        checkPlayerParameter(player);
        return playerService.craft(player);
    }

    @POST
    @Path("/destroy")
    @Transactional
    public World destroy(@NotNull Player player) {
        checkPlayerParameter(player);
        return playerService.destroy(player);
    }

    @GET
    @Path("/inventory")
    public Inventory inventory() {
        return playerService.getInventory();
    }

    @GET
    @Path("/messages")
    public CharacterMessages messages() {
        CharacterMessages messages = new CharacterMessages();
        messages.getMessages().addAll(characterMessageService.getMessages());
        return messages;
    }

    @POST
    @Transactional
    @Path("/message")
    public void message(@NotNull CharacterMessage message) {
        checkMessageParameter(message);
        characterMessageService.createMessage(message);
    }

    @POST
    @Transactional
    @Path("/update")
    public Player update(@NotNull Transform newTransform) {
        return playerService.update(newTransform);
    }
}
