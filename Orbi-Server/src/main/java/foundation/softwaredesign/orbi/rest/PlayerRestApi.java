package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.Inventory;
import foundation.softwaredesign.orbi.model.Player;
import foundation.softwaredesign.orbi.model.Transform;
import foundation.softwaredesign.orbi.model.World;
import foundation.softwaredesign.orbi.service.ElevationService;
import foundation.softwaredesign.orbi.service.PlayerService;

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

    @POST
    @Transactional
    @Path("/init")
    public Player player(@NotNull Transform newTransform) {
        return playerService.init(newTransform);
    }
}
