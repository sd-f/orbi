package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.real.Position;
import foundation.softwaredesign.orbi.model.virtual.Cube;
import foundation.softwaredesign.orbi.model.virtual.World;

import javax.enterprise.context.ApplicationScoped;

import java.math.BigDecimal;

import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@ApplicationScoped
public class WorldAdapter {

    private Boolean isWorldOk(World world) {
        if (nonNull(world) && nonNull(world.getCubes()) && !world.getCubes().isEmpty()) {
            return true;
        }
        return false;
    }

    private Boolean isPositionOk(Position position) {
        if (nonNull(position)) {
            if (nonNull(position.getElevation())
                    && nonNull(position.getLatitude())
                && nonNull(position.getLongitute())) {
                return true;
            }
        }
        return false;
    }

    private Boolean isCubeOk(Cube cube) {
        if (nonNull(cube) && nonNull(cube.getCoordinates())) {
            if (nonNull(cube.getCoordinates().getX())
                    && nonNull(cube.getCoordinates().getY())
                    && nonNull(cube.getCoordinates().getZ())) {
                return true;
            }
        }
        return false;
    }

    private BigDecimal scaleToVirtual(BigDecimal real, BigDecimal position, Integer scale) {
        BigDecimal newCoordinate = real.subtract(position);
        return newCoordinate.multiply(new BigDecimal(scale));
    }

    private BigDecimal scaleToReal(BigDecimal virtual, BigDecimal position, Integer scale) {
        BigDecimal newCoordinate = virtual.divide(new BigDecimal(scale));
        return newCoordinate.add(position);
    }

    public void convertToVirtual(World world, Position position) {
        if (isWorldOk(world) && isPositionOk(position)) {
            world.getCubes()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(cube -> {
                        cube.getCoordinates().setX(
                                scaleToVirtual(cube.getCoordinates().getX(), position.getLongitute(), 50000)
                        );
                        cube.getCoordinates().setY(
                                scaleToVirtual(cube.getCoordinates().getY(), position.getElevation(), 1)
                        );
                        cube.getCoordinates().setZ(
                                scaleToVirtual(cube.getCoordinates().getZ(), position.getLatitude(), 50000)
                        );
            });
        }
    }

    public void convertToReal(World world, Position position) {
        if (isWorldOk(world) && isPositionOk(position)) {
            world.getCubes()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(cube -> {
                        cube.getCoordinates().setX(
                                scaleToReal(cube.getCoordinates().getX(), position.getLongitute(), 50000)
                        );
                        cube.getCoordinates().setY(
                                scaleToReal(cube.getCoordinates().getY(), position.getElevation(), 1)
                        );
                        cube.getCoordinates().setZ(
                                scaleToReal(cube.getCoordinates().getZ(), position.getLatitude(), 50000)
                        );
                    });
        }
    }
}
