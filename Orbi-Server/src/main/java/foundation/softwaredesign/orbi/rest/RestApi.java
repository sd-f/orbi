package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
import foundation.softwaredesign.orbi.model.virtual.World;
import foundation.softwaredesign.orbi.persistence.repo.ElevationRepository;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;
import foundation.softwaredesign.orbi.service.WorldAdapter;
import foundation.softwaredesign.orbi.service.WorldFactory;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;
import java.math.BigDecimal;
import java.util.List;

import static java.util.Objects.nonNull;
import static javax.ws.rs.core.MediaType.APPLICATION_JSON;
import static javax.ws.rs.core.MediaType.APPLICATION_XML;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/")
@Produces({APPLICATION_XML, APPLICATION_JSON})
@RequestScoped
public class RestApi {

    @Inject
    GameObjectRepository gameObjectRepository;

    @Inject
    ElevationRepository elevationRepository;

    @Inject
    WorldAdapter worldAdapter;

    @Inject
    WorldFactory worldFactory;

    @GET
    @Path("/init")
    @Produces(MediaType.TEXT_PLAIN)
    public String init() {
        worldFactory.init();
        return "OK";
    }

    @GET
    @Path("/elevation")
    @Produces(MediaType.TEXT_PLAIN)
    public String elevation(@NotNull(message = "latitude required") @QueryParam("latitude") BigDecimal latitude,
                            @NotNull(message = "longitude required") @QueryParam("longitude") BigDecimal longitude) {

        Double elevation = elevationRepository.getElevation(latitude, longitude);
        if (nonNull(elevation)) {
            return elevation.toString();
        }
        return null;
    }

    @GET
    @Path("/world")
    public World world(@NotNull(message = "latitude required") @QueryParam("latitude") BigDecimal latitude,
                       @NotNull(message = "longitude required") @QueryParam("longitude") BigDecimal longitude) {
        System.out.println("requesting world");
        return getWorld(latitude, longitude);
    }

    @POST
    @Path("/create")
    @Consumes({APPLICATION_XML, APPLICATION_JSON})
    public World create(@NotNull(message = "latitude required") @QueryParam("latitude") BigDecimal latitude,
                        @NotNull(message = "longitude required") @QueryParam("longitude") BigDecimal longitude,
                        World world) {
        System.out.println("creating cubes");
        worldAdapter.convertToReal(world, getPosition(latitude, longitude));
        for (GameObject gameObject : world.getGameObjects()) {
            gameObjectRepository.save(gameObject);
            System.out.println("Saved gameObject");
        }
        return getWorld(latitude, longitude);
    }

    private World getWorld(BigDecimal latitude, BigDecimal longitude) {
        World world = new World();
        List<GameObject> gameObjectList = gameObjectRepository.findCubesAround(latitude, longitude);
        world.setGameObjects(gameObjectList);
        // TODO elevation
        foundation.softwaredesign.orbi.model.real.Position position = getPosition(latitude, longitude);
        worldAdapter.convertToVirtual(world, position);
        return world;
    }

    private foundation.softwaredesign.orbi.model.real.Position getPosition(BigDecimal latitude, BigDecimal longitude) {
        return new foundation.softwaredesign.orbi.model.real.Position(latitude, longitude, new BigDecimal(0.0));
    }

}
